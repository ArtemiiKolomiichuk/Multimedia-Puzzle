using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button3D : MonoBehaviour
{
    [SerializeField] UnityEvent onClick;
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
        bool invoked = false;
        Vector3 initialPosition = transform.localPosition;
        float timePassed = 0f;
        while (timePassed < len)
        {
            if(!invoked && timePassed > len * 0.7f)
            {
                onClick.Invoke();
                invoked = true;
            }
            timePassed += Time.deltaTime * speed;
            var newPosition = initialPosition;
            newPosition.y = curve.Evaluate(timePassed);
            transform.localPosition = Vector3.Lerp(initialPosition, newPosition, timePassed/len);
            yield return null;
        }
        isAnimating = false;
        if(!invoked)
            onClick.Invoke();
    }
}
