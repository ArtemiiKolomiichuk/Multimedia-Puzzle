using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Collider))]
public class FocusPoint : MonoBehaviour
{
    private Collider clickCollider;
    [SerializeField] private Vector3 cameraPosition;
    [SerializeField] private Vector3 cameraRotation;

    void Start()
    {
        clickCollider = GetComponent<Collider>();
    }

    private void OnMouseDown()
    {
        CameraController.Instance.MoveCam(cameraPosition, cameraRotation);
        clickCollider.enabled = false;
    }

    public void EnablePoint()
    {
        clickCollider.enabled = true;
    }
}
