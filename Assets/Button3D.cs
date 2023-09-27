using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button3D : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    bool isAnimating = false;
    public float len;
    public float speed;

    private void OnMouseDown()
    {
        if(!isAnimating)
            AnimateClick();
    }

    private void AnimateClick()
    {
        isAnimating = true;
        StartCoroutine(AnimateClickCoroutine());
    }

    private IEnumerator AnimateClickCoroutine()
    {
        Vector3 initialPosition = transform.localPosition;
        float timePassed = 0f;
        while (timePassed < len)
        {
            timePassed += Time.deltaTime * speed;
            var newPosition = initialPosition;
            newPosition.y = curve.Evaluate(timePassed);
            transform.localPosition = Vector3.Lerp(initialPosition, newPosition, timePassed/len);
            yield return null;
        }
        isAnimating = false;
    }
}
