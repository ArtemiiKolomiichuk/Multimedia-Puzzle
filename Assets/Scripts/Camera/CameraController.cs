using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    private bool isMoving = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private bool isInitialPosition = true;
    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] private float speed = 1f;

    public FocusPointType currentFocusPoint = FocusPointType.Other;

    public enum FocusPointType
    {
        Board,
        Other
    }

    private void Start()
    {
        Instance = this;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void MoveCam()
    {
        if(!isMoving && !isInitialPosition)
        {
            StartCoroutine(MoveCameraCoroutine(initialPosition, initialRotation.eulerAngles));
        }
    }

    public void MoveCam(Vector3 targetPos, Vector3 targetRot)
    {
        if (!isMoving)
        {
            if (!isInitialPosition)
            {
                Debug.LogWarning("Camera is not in initial position");
                //StartCoroutine(MoveCameraCoroutine(initialPosition, initialRotation.eulerAngles));
            }
            else
            {
                StartCoroutine(MoveCameraCoroutine(targetPos, targetRot));
            }
        }
    }

    private IEnumerator MoveCameraCoroutine(Vector3 targetPos, Vector3 targetRot)
    {
        isMoving = true;

        Vector3 initialPosition = transform.position;
        Quaternion initialRotation = transform.rotation;

        float timePassed = 0f;
        float trueTimePassed = 0f;
        while (timePassed < 1)
        {
            trueTimePassed += Time.deltaTime;
            timePassed += Time.deltaTime * speed * speedCurve.Evaluate(trueTimePassed);
            transform.SetPositionAndRotation(
                Vector3.Lerp(initialPosition, targetPos, timePassed), 
                Quaternion.Slerp(initialRotation, Quaternion.Euler(targetRot), timePassed));
            yield return null;
        }

        transform.SetPositionAndRotation(targetPos, Quaternion.Euler(targetRot));
        isInitialPosition = !isInitialPosition;
        if (isInitialPosition)
        {
            var points = FindObjectsByType<FocusPoint>(FindObjectsSortMode.None);
            foreach(var point in points)
            {
                point.EnablePoint();
            }
        }
        Camera.main.GetComponent<CameraFocusUI>().EnableDisable();
        isMoving = false;
    }
}

