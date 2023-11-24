using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private GameObject lamp;
    [SerializeField] private Material litLamp;
    [SerializeField] private GameObject lever;

    public void TurnOnLamp()
    {
        lamp.GetComponent<MeshRenderer>().material = litLamp;
        lamp.GetComponent<Light>().enabled = true;
    }

    public void BreakLever()
    {
        var leverScript = lever.GetComponent<Lever>();
        leverScript.BreakLever();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
