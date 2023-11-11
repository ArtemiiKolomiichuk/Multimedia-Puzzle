using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampsController : MonoBehaviour
{
    [SerializeField] private GameObject[] lamps;
    [SerializeField] private bool[] lampsOn;
    [SerializeField] private Material[] lampOn;
    [SerializeField] private Material lampOff;
    [SerializeField] private Lever[] levers;

    public static LampsController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var lamp in lamps)
        {
            var m = lamp.GetComponent<MeshRenderer>().material;
            m.EnableKeyword("_EMISSION");
        }
        lamps[0].GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
    }

    public static void LightLamps(int lever)
    {
        switch (lever)
        {
            case 0:
                Instance.lampsOn[0] = !Instance.lampsOn[0];
                if (!Instance.lampsOn[0])
                {
                    Instance.lamps[0].GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
                }
                else
                {
                    Instance.lamps[0].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                }  
                Instance.lampsOn[1] = !Instance.lampsOn[1];
                if (!Instance.lampsOn[1])
                {
                    Instance.lamps[1].GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
                }
                else
                {
                    Instance.lamps[1].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                }
                Instance.lampsOn[4] = !Instance.lampsOn[4];
                if (!Instance.lampsOn[4])
                {
                    Instance.lamps[4].GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
                }
                else
                {
                    Instance.lamps[4].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                }
                break;
            case 1:
                Instance.lampsOn[1] = !Instance.lampsOn[1];
                if (!Instance.lampsOn[1])
                {
                    Instance.lamps[1].GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
                }
                else
                {
                    Instance.lamps[1].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                }
                Instance.lampsOn[2] = !Instance.lampsOn[2];
                if (!Instance.lampsOn[2])
                {
                    Instance.lamps[2].GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
                }
                else
                {
                    Instance.lamps[2].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                }
                Instance.lampsOn[5] = !Instance.lampsOn[5];
                if (!Instance.lampsOn[5])
                {
                    Instance.lamps[5].GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
                }
                else
                {
                    Instance.lamps[5].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                }
                break;
            case 2:
                Instance.lampsOn[0] = !Instance.lampsOn[0];
                if (!Instance.lampsOn[0])
                {
                    Instance.lamps[0].GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
                }
                else
                {
                    Instance.lamps[0].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                }
                Instance.lampsOn[2] = !Instance.lampsOn[2];
                if (!Instance.lampsOn[2])
                {
                    Instance.lamps[2].GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
                }
                else
                {
                    Instance.lamps[2].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                }
                Instance.lampsOn[3] = !Instance.lampsOn[3];
                if (!Instance.lampsOn[3])
                {
                    Instance.lamps[3].GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
                }
                else
                {
                    Instance.lamps[3].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                }
                break;
            case 3:
                Instance.lampsOn[1] = !Instance.lampsOn[1];
                if (!Instance.lampsOn[1])
                {
                    Instance.lamps[1].GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
                }
                else
                {
                    Instance.lamps[1].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                }
                Instance.lampsOn[3] = !Instance.lampsOn[3];
                if (!Instance.lampsOn[3])
                {
                    Instance.lamps[3].GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
                }
                else
                {
                    Instance.lamps[3].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                }
                Instance.lampsOn[5] = !Instance.lampsOn[5];
                if (!Instance.lampsOn[5])
                {
                    Instance.lamps[5].GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
                }
                else
                {
                    Instance.lamps[5].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                }
                break;
        }
        Instance.CheckWin();
    }

    private void CheckWin()
    {
        if (lampsOn[0] && lampsOn[1] && lampsOn[2] && lampsOn[3] && lampsOn[4] && lampsOn[5])
        {
            StartCoroutine(CheckWinCoroutine());
        }
    }

    IEnumerator CheckWinCoroutine()
    {
        yield return new WaitForSeconds(0.6f);
        foreach (var lamp in lamps)
        {
            lamp.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        }
        var damagedLamps = FindObjectsByType<DamagedLamp>(FindObjectsSortMode.None);
        foreach (var lamp in damagedLamps)
        {
            lamp.brokenLamp.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
            lamp.DestroyLamp();
        }
        foreach (var lever in levers)
        {
            lever.BreakLever();
        }
    }
}
