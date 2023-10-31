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

    [SerializeField] private Sprite[] onVisitedSprites;

    [SerializeField] private bool flippable = true;
    [SerializeField] private bool isOceanic = false;

    public void Flip(Direction direction, float duration = -1, float height = -1)
    {
        if(!flippable)
            return;
        if(duration == -1)
            duration = flipDuration;
        if(height == -1)
            height = yMax;
        RotateUnderlyingImage(direction);
        StartCoroutine(FlipCoroutine(direction, duration, height));
        flippable = false;
    }

    private void RotateUnderlyingImage(Direction direction)
    {
        if (transform.childCount < 2)
        {
            return;
        }
        if(direction == Direction.Left || direction == Direction.Right)
        {
            transform.GetComponentsInChildren<SpriteRenderer>()[1].flipY = true;
            transform.GetComponentsInChildren<SpriteRenderer>()[1].flipX = true;
            var s = transform.GetComponentsInChildren<SpriteRenderer>()[2];
            s.sprite = onVisitedSprites[(isOceanic ? 0 : 1)];
            s.flipY = true;
            s.flipX = true;
            s.transform.localPosition = new Vector3(-s.transform.localPosition.x, s.transform.localPosition.y, -s.transform.localPosition.z);
        }        
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
        GetComponent<MeshRenderer>().material = secondMaterial;
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
