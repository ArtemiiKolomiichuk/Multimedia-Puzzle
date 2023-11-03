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
                     private Material firstMaterial;
    [SerializeField] internal bool flipped = false;
    [SerializeField] private bool isOceanic = false;

    [NonSerialized] public Material intermidiateMaterial;

    private void Start()
    {
        firstMaterial = GetComponent<MeshRenderer>().material;
    }

    public void Flip(Direction direction, float duration = -1, float height = -1, Action callback = null)
    {
        if (flipped)
        {
            callback?.Invoke();
            return;
        }
            
        if(duration == -1)
            duration = flipDuration;
        if(height == -1)
            height = yMax;
        SetUnderlyingImage(direction);
        StartCoroutine(FlipCoroutine(direction, duration, height, callback));
        StartCoroutine(LerpMaterialColor(intermidiateMaterial, secondMaterial.color, 1.1f, secondMaterial));
        flipped = true;
    }

    public void ForceFlip(Direction direction, float duration = -1, float height = -1)
    {
        if (duration == -1)
            duration = flipDuration;
        if (height == -1)
            height = yMax;
        StartCoroutine(FlipCoroutine(direction, duration, height));
        GetComponent<MeshRenderer>().material = intermidiateMaterial;

        if (transform.childCount == 0){}
        else
        {
            if (transform.childCount > 1)
            {
                var topGO = transform.GetChild(1).gameObject;
                var top = topGO.GetComponent<SpriteRenderer>();
                top.flipX = false;
                top.flipY = true;
            }
            else
            {
                SetUnderlyingImage(direction);
            }
        }
    }

    public void ResetTile(Direction direction, float duration = -1, float height = -1)
    {
        flipped = false;
        GetComponent<MeshRenderer>().material = firstMaterial;
        StartCoroutine(ResetFlipCoroutine(direction, duration, height));
    }

    private IEnumerator ResetFlipCoroutine(Direction direction, float duration, float height)
    {
        var baseRot = transform.localRotation;
        var finalRot = Quaternion.Euler(ResetNewRotation(direction));
        float timePassed = 0f;
        while (timePassed < 1)
        {
            timePassed += Time.deltaTime * 1 / duration;

            transform.SetLocalPositionAndRotation(new Vector3(transform.localPosition.x, height * yCurve.Evaluate(timePassed) + baseY, transform.localPosition.z), 
                Quaternion.Lerp(baseRot, finalRot, rotCurve.Evaluate(timePassed)));
            yield return null;
        }
    }

    private Vector3 ResetNewRotation(Direction direction)
    {
        return direction switch
        {
            Direction.Up => new Vector3(0, 0, 0),
            Direction.Down => new Vector3(0, 180, 0),
            Direction.Left => new Vector3(0, 90, 0),
            Direction.Right => new Vector3(0, -90, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private void SetUnderlyingImage(Direction direction)
    {
        SpriteRenderer bottom;
        if (transform.childCount < 2)
        {
            if (transform.childCount != 1)
            {
                return;
            }
            var bottomGO = Instantiate(transform.GetChild(0).gameObject, transform);
            bottomGO.transform.localPosition = new Vector3(0, -0.51f, 0);
            bottom = bottomGO.GetComponent<SpriteRenderer>();
        }
        else
        {
            bottom = transform.GetChild(1).GetComponent<SpriteRenderer>();
            bottom.flipX = false;
            bottom.flipY = false;
        }
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

    IEnumerator LerpMaterialColor(Material intermidiateMaterial, Color targetColor, float duration, Material newMaterial)
    {
        intermidiateMaterial.color = isOceanic ? new Color(0.062f,0.832f,0.941f) : new Color(0.8627f, 0.6156f, 0.203f); 
        var baseColor = intermidiateMaterial.color;
        GetComponent<MeshRenderer>().material = intermidiateMaterial;
        float timePassed = 0f;
        while (timePassed < 1)
        {
            timePassed += Time.deltaTime * 1/duration;
            intermidiateMaterial.color = Color.Lerp(baseColor, targetColor, timePassed);
            yield return null;
        }
        this.GetComponent<MeshRenderer>().material = newMaterial;
    }

    private IEnumerator FlipCoroutine(Direction direction, float duration, float height, Action callback = null)
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
        callback?.Invoke();
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
