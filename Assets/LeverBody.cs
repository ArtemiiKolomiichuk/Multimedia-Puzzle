using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverBody : MonoBehaviour
{
    public UnityAction leverClicked;

    private void OnMouseDown()
    {
        leverClicked?.Invoke();
    }
}
