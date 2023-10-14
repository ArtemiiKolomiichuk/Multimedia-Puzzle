using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    public void EnableDisable()
    {
        canvas.enabled = !canvas.enabled;
    }
}
