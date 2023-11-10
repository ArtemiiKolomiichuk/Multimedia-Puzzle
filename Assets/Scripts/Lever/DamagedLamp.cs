using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedLamp : MonoBehaviour
{
    private bool blownUp = false;
    [SerializeField] internal GameObject brokenLamp;
    [SerializeField] private ParticleSystem[] particleSystems;

    private void Awake()
    {
        foreach (var particles in particleSystems)
        {
            particles.Stop();
        }
    }

    internal void DestroyLamp()
    {
        if (blownUp)
            return;
        blownUp = true;
        StartCoroutine(BlowUp());
    }

    private IEnumerator BlowUp()
    {
        foreach (var particles in particleSystems)
        {
            particles.Play();
        }
        yield return new WaitForSeconds(0.2f);
        brokenLamp.SetActive(true);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
