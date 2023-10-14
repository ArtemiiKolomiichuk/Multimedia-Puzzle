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

    public void Flip(Direction direction, float duration = -1, float height = -1)
    {
        if(duration == -1)
            duration = flipDuration;
        if(height == -1)
            height = yMax;
        StartCoroutine(FlipCoroutine(direction, duration, height));
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
