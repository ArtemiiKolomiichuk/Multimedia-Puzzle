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
    private GameObject pin = null;
    private int pinState = 0;
    private bool isPinMoving = false;
    internal (int, int) coordinates;

    private void Start()
    {
        firstMaterial = GetComponent<MeshRenderer>().material;
    }

    private void OnMouseDown()
    {
        if (isPinMoving)
            return;
        isPinMoving = true;
        if (pin == null)
        {
            var pin = Instantiate(BoardController.Instance.pin);
            this.pin = pin;
            var newRotation = new Vector3(20.356f, -33.953f, -30.06f);
            newRotation.z += 4.5667f * (coordinates.Item1 - 3);
            newRotation.x -= 4.9667f * (coordinates.Item2 - 3);
            pin.transform.localEulerAngles = newRotation;
            StartCoroutine(MovePin(pin, true));
        }
        else
        {
            pinState++;
            if (pinState == 3)
            {
                StartCoroutine(MovePin(pin, false, () => { Destroy(pin); this.pin = null; }));
                pinState = 0;
            }
            else
            {
                pin.GetComponent<MeshRenderer>().material = BoardController.Instance.pinMaterials[pinState];
                isPinMoving = false;
            }
        }
    }

    private void HidePin()
    {
        if (pin == null)
            return;
        StartCoroutine(MovePin(pin, false, () => { pin.SetActive(false); }));
    }

    private void ShowPin()
    {
        if (pin == null)
            return;
        pin.SetActive(true);
        StartCoroutine(MovePin(pin, true));
    }

    const float pinDelta = 1.19f;

    private IEnumerator MovePinForFlip()
    {
        if (pin == null)
        {
            yield break;
        }
        var duration = flipDuration * 1.2f;
        duration /= 2;
        var timePassed = 0f;
        var startPos = new Vector3(-108.033f, -1.189f, 21.295f);
        startPos.x += pinDelta * (coordinates.Item1 - 3);
        startPos.z += pinDelta * (coordinates.Item2 - 3);
        var endPos = new Vector3(-107.579f, 0.302f, 22.268f);
        endPos.x += pinDelta * (coordinates.Item1 - 3);
        endPos.z += pinDelta * (coordinates.Item2 - 3);
        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            var t = timePassed / duration / 2;
            pin.transform.localPosition = Vector3.Lerp(startPos, endPos, t/1.4f);
            yield return null;
        }
        timePassed = 0f;
        var newPos = pin.transform.localPosition;
        
        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            var t = timePassed / duration;
            pin.transform.localPosition = Vector3.Lerp(newPos, startPos, t);
            yield return null;
        }

    }

    private IEnumerator MovePin(GameObject pin, bool put, Action callback = null)
    {
        var startPos = new Vector3(-108.033f, -1.189f, 21.295f);
        startPos.x += pinDelta * (coordinates.Item1 - 3);
        startPos.z += pinDelta * (coordinates.Item2 - 3);
        var endPos = new Vector3(-107.579f, 0.302f, 22.268f);
        endPos.x += pinDelta * (coordinates.Item1 - 3);
        endPos.z += pinDelta * (coordinates.Item2 - 3);

        if (put)
        {
            (endPos, startPos) = (startPos, endPos);
        }

        var duration = 0.12f;
        var timePassed = 0f;
        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            var t = timePassed / duration;
            pin.transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        callback?.Invoke();
        isPinMoving = false;
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
        StartCoroutine(MovePinForFlip());
        StartCoroutine(FlipCoroutine(direction, duration, height, callback));
        if(coordinates == (3, 3))
        {
            StartCoroutine(LerpMaterialFully(intermidiateMaterial, secondMaterial.color, 0.8f, secondMaterial));
        }
        else
        {
            StartCoroutine(LerpMaterialColor(intermidiateMaterial, secondMaterial.color, 1.1f, secondMaterial));
        }
        flipped = true;
    }

    private IEnumerator LerpMaterialFully(Material intermidiateMaterial, Color color, float duration, Material secondMaterial)
    {
        var goalSmoothness = secondMaterial.GetFloat("_Smoothness");
        var goalMettalic = secondMaterial.GetFloat("_Metallic");
        var baseColor = GetComponent<MeshRenderer>().material.color;
        var goalColor = secondMaterial.color;
        var timePassed = 0f;
        intermidiateMaterial.color = baseColor;
        this.GetComponent<MeshRenderer>().material = intermidiateMaterial;
        while (timePassed < 1)
        {
            timePassed += Time.deltaTime * 1/duration;
            intermidiateMaterial.SetFloat("_Smoothness", Mathf.Lerp(0, goalSmoothness, timePassed));
            intermidiateMaterial.SetFloat("_Metallic", Mathf.Lerp(0, goalMettalic, timePassed));
            intermidiateMaterial.color = Color.Lerp(baseColor, goalColor, timePassed);
            yield return null;
        }
        this.GetComponent<MeshRenderer>().material = secondMaterial;
        intermidiateMaterial.SetFloat("_Smoothness", 0);
        intermidiateMaterial.SetFloat("_Metallic", 0);
    }

    public void ForceFlip(Direction direction, float duration = -1, float height = -1)
    {
        HidePin();
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
        ShowPin();
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
            if(timePassed > 0.2f)
            {
                callback?.Invoke();
                callback = null;
            }
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
