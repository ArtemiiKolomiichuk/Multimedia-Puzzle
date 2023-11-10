using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    private bool isMoving = false;
    [SerializeField] private bool isPulled = false;
    [SerializeField] private AnimationCurve extentCurve;
    [SerializeField] private float duration = 1f;
    [SerializeField] UnityEvent onPulled;
    [SerializeField] UnityEvent onPushed;

    private void Start()
    {
        transform.parent.GetComponent<LeverBody>().leverClicked += PullPush;
    }

    void PullPush()
    {
        if(isMoving)
            return;
        if(isBroken)
            return;
        if(!isPulled)
        {
            StartCoroutine(MoveLever(Quaternion.Euler(-36, 0, 0)));
            onPushed?.Invoke();
        }
        else
        {
            StartCoroutine(MoveLever(Quaternion.Euler(36, 0, 0)));
            onPulled?.Invoke();
        }
    }

    IEnumerator MoveLever(Quaternion rotation)
    {
        isMoving = true;
        Quaternion initialRotation = transform.localRotation;

        float timePassed = 0f;
        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(initialRotation, rotation, extentCurve.Evaluate(timePassed/duration));
            yield return null;
        }

        isPulled = !isPulled;
        isMoving = false;
    }

    private bool isBroken = false;
    internal void BreakLever()
    {
        if (isBroken)
            return;
        isBroken = true;
        isMoving = true;
        onPulled = null;
        onPushed = null;
        transform.parent.GetComponent<LeverBody>().leverClicked = null;
        //StartCoroutine(Break());
    }

    private IEnumerator Break()
    {
        throw new NotImplementedException();
    }
}
