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
    int clipNum = -1;
    void Start()
    {
        radioScript = radio.GetComponent<RadioScript>();
        text = textObject.GetComponent<TextMeshProUGUI>();
        if(radioScript.audioClips != null) text.text = radioScript.audioClips[radioScript.clipNum].name;
    }

    // Update is called once per frame
    void Update()
    {
        if (radioScript.isStopped && playObject.transform.localScale.x == 0)
        {
            stopObject.transform.localScale = Vector3.zero;
            playObject.transform.localScale = Vector3.one;
        }
        else if (!radioScript.isStopped && !(playObject.transform.localScale.x == 0))
        {
            stopObject.transform.localScale = Vector3.one;
            playObject.transform.localScale = Vector3.zero;
        }
        if(clipNum != radioScript.clipNum) text.text = radioScript.songTitles[radioScript.clipNum];
        clipNum = radioScript.clipNum;
    }
}
