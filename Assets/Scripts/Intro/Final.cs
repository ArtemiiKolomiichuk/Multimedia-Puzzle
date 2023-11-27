using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(End());
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(11);
        Application.Quit();
    }
}
