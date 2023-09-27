using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    private bool isMoving = false;
    [SerializeField] private bool isPulled = false;
    [SerializeField] private AnimationCurve extentCurve;
    [SerializeField] private float duration = 1f;

    private void Start()
    {
        transform.parent.GetComponent<LeverBody>().leverClicked += PullPush;
    }

    void PullPush()
    {
        if(isMoving)
            return;
        if(!isPulled)
        {
            StartCoroutine(MoveLever(Quaternion.Euler(-36, 0, 0)));
        }
        else
        {
            StartCoroutine(MoveLever(Quaternion.Euler(36, 0, 0)));
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
}
