using UnityEngine;

public class VideoPlayerButtonScript : MonoBehaviour
{
    [SerializeField]
    bool isNextButton = false;
    private void OnMouseDown()
    {
        if(isNextButton)
        transform.parent.parent.GetComponent<VideoPlayerScript>().Next();
        else
        transform.parent.parent.GetComponent<VideoPlayerScript>().Back();
    }
}
