using TMPro;
using UnityEngine;

public class RadioScreenScript : MonoBehaviour
{
    [SerializeField]
    public GameObject radio;
    private RadioScript radioScript;
    [SerializeField]
    public GameObject stopObject;
    [SerializeField]
    public GameObject playObject;
    [SerializeField]
    public GameObject textObject;
    private TextMeshProUGUI text;
    void Start()
    {
        radioScript = radio.GetComponent<RadioScript>();
        text = textObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (radioScript.isStopped && stopObject.transform.localScale.x == 0)
        {
            stopObject.transform.localScale = Vector3.one;
            playObject.transform.localScale = Vector3.zero;
        }
        else if (!radioScript.isStopped && !(stopObject.transform.localScale.x == 0))
        {
            stopObject.transform.localScale = Vector3.zero;
            playObject.transform.localScale = Vector3.one;
            text.text = radioScript.audioClips[radioScript.clipNum].name;
        }
    }
}
