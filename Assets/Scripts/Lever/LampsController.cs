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

    public static LampsController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public static void LightLamps(int lever)
    {
        switch (lever)
        {
            case 0:
                Instance.lampsOn[0] = !Instance.lampsOn[0];
                Instance.lamps[0].GetComponent<MeshRenderer>().material = Instance.lampsOn[0] ? Instance.lampOn[0] : Instance.lampOff;
                Instance.lampsOn[1] = !Instance.lampsOn[1];
                Instance.lamps[1].GetComponent<MeshRenderer>().material = Instance.lampsOn[1] ? Instance.lampOn[1] : Instance.lampOff;
                Instance.lampsOn[4] = !Instance.lampsOn[4];
                Instance.lamps[4].GetComponent<MeshRenderer>().material = Instance.lampsOn[4] ? Instance.lampOn[4] : Instance.lampOff;
                break;
            case 1:
                Instance.lampsOn[1] = !Instance.lampsOn[1];
                Instance.lamps[1].GetComponent<MeshRenderer>().material = Instance.lampsOn[1] ? Instance.lampOn[1] : Instance.lampOff;
                Instance.lampsOn[2] = !Instance.lampsOn[2];
                Instance.lamps[2].GetComponent<MeshRenderer>().material = Instance.lampsOn[2] ? Instance.lampOn[2] : Instance.lampOff;
                Instance.lampsOn[5] = !Instance.lampsOn[5];
                Instance.lamps[5].GetComponent<MeshRenderer>().material = Instance.lampsOn[5] ? Instance.lampOn[5] : Instance.lampOff;
                break;
            case 2:
                Instance.lampsOn[0] = !Instance.lampsOn[0];
                Instance.lamps[0].GetComponent<MeshRenderer>().material = Instance.lampsOn[0] ? Instance.lampOn[0] : Instance.lampOff;
                Instance.lampsOn[2] = !Instance.lampsOn[2];
                Instance.lamps[2].GetComponent<MeshRenderer>().material = Instance.lampsOn[2] ? Instance.lampOn[2] : Instance.lampOff;
                Instance.lampsOn[3] = !Instance.lampsOn[3];
                Instance.lamps[3].GetComponent<MeshRenderer>().material = Instance.lampsOn[3] ? Instance.lampOn[3] : Instance.lampOff;
                break;
            case 3:
                Instance.lampsOn[1] = !Instance.lampsOn[1];
                Instance.lamps[1].GetComponent<MeshRenderer>().material = Instance.lampsOn[1] ? Instance.lampOn[1] : Instance.lampOff;
                Instance.lampsOn[3] = !Instance.lampsOn[3];
                Instance.lamps[3].GetComponent<MeshRenderer>().material = Instance.lampsOn[3] ? Instance.lampOn[3] : Instance.lampOff;
                Instance.lampsOn[5] = !Instance.lampsOn[5];
                Instance.lamps[5].GetComponent<MeshRenderer>().material = Instance.lampsOn[5] ? Instance.lampOn[5] : Instance.lampOff;
                break;
        }
        Instance.CheckWin();
    }

    private void CheckWin()
    {
        if (lampsOn[0] && lampsOn[1] && lampsOn[2] && lampsOn[3] && lampsOn[4] && lampsOn[5])
        {
            Destroy(lamps[0]);
            Destroy(lamps[3]);
            Destroy(lamps[4]);
        }
    }
}
