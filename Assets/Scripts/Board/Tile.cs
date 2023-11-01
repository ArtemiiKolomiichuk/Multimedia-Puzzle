using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private AnimationCurve yCurve;
    [SerializeField] private float baseY;
    [SerializeField] private float yMax;
    [SerializeField] private AnimationCurve rotCurve;
    [SerializeField] private float maxRot;
    [SerializeField] private float flipDuration;
    [SerializeField] private Material secondMaterial;

    [SerializeField] private bool flippable = true;
    [SerializeField] private bool isOceanic = false;

    [NonSerialized] public Material intermidiateMaterial; 

    public void Flip(Direction direction, float duration = -1, float height = -1)
    {
        if(!flippable)
            return;
        if(duration == -1)
            duration = flipDuration;
        if(height == -1)
            height = yMax;
        SetUnderlyingImage(direction);
        StartCoroutine(FlipCoroutine(direction, duration, height));
        StartCoroutine(LerpMaterialColor(intermidiateMaterial, secondMaterial.color, 1.1f));
        flippable = false;
    }

    private void SetUnderlyingImage(Direction direction)
    {
        if (transform.childCount != 1)
        {
            return;
        }
        var bottomGO = Instantiate(transform.GetChild(0).gameObject, transform);
        bottomGO.transform.localPosition = new Vector3(0, -0.51f, 0);
        var bottom = bottomGO.GetComponent<SpriteRenderer>();
        if(direction == Direction.Left || direction == Direction.Right)
        {
            bottom.flipX = true;
        }
        else
        {
            bottom.flipY= true;
        }
        StartCoroutine(FadeImage(bottom));
    }

    IEnumerator FadeImage(SpriteRenderer spriteRenderer)
    {
        yield return new WaitForSeconds(0.6f);
        float timePassed = 0f;
        float duration = 0.5f;
        float targetAlpha = 0.6f;
        while (timePassed < 1)
        {
            timePassed += Time.deltaTime * 1/duration;
            spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(1, targetAlpha, timePassed));
            yield return null;
        }
    }

    IEnumerator LerpMaterialColor(Material intermidiateMaterial, Color targetColor, float duration)
    {
        intermidiateMaterial.color = GetComponent<MeshRenderer>().material.color;
        var baseColor = intermidiateMaterial.color;
        GetComponent<MeshRenderer>().material = intermidiateMaterial;
        float timePassed = 0f;
        while (timePassed < 1)
        {
            timePassed += Time.deltaTime * 1/duration;
            intermidiateMaterial.color = Color.Lerp(baseColor, targetColor, timePassed);
            yield return null;
        }
        this.GetComponent<MeshRenderer>().material = secondMaterial;
    }

    private IEnumerator FlipCoroutine(Direction direction, float duration, float height)
    {
        float timePassed = 0f;
        while (timePassed < 1)
        {
            timePassed += Time.deltaTime * 1/duration;
            transform.SetLocalPositionAndRotation(
                new Vector3(transform.localPosition.x, height * yCurve.Evaluate(timePassed) + baseY, transform.localPosition.z), 
                Quaternion.Euler(NewRotation(direction, timePassed)));
            yield return null;
        }
    }

    private Vector3 NewRotation(Direction direction, float timePassed)
    {
        return direction switch
        {
            Direction.Up => new Vector3(+1 * rotCurve.Evaluate(timePassed) * maxRot, 0, 0),
            Direction.Down => new Vector3(-1 * rotCurve.Evaluate(timePassed) * maxRot, 0, 0),
            Direction.Left => new Vector3(0, 0, +1 * rotCurve.Evaluate(timePassed) * maxRot),
            Direction.Right => new Vector3(0, 0, -1 * rotCurve.Evaluate(timePassed) * maxRot),
            _ => Vector3.zero,
        };
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
