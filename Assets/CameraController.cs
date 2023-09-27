using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool isMoving = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private bool isInitialPosition = true;
    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] private float speed = 1f;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            if(!isInitialPosition)
            {
                StartCoroutine(MoveCameraSmoothly(initialPosition, initialRotation.eulerAngles));
            }
            else
            {
                StartCoroutine(MoveCameraSmoothly(new Vector3(-107.895f,7.885f,21.755f), new Vector3(93.611f,-0.838f,-0.013f)));
            }
            isInitialPosition = !isInitialPosition;
        }
    }

    private IEnumerator MoveCameraSmoothly(Vector3 targetPos, Vector3 targetRot)
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
            transform.position = Vector3.Lerp(initialPosition, targetPos, timePassed);
            transform.rotation = Quaternion.Slerp(initialRotation, Quaternion.Euler(targetRot), timePassed);
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = Quaternion.Euler(targetRot);

        isMoving = false;
    }
}

