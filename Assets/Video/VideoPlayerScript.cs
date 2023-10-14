using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerScript : MonoBehaviour
{
    [SerializeField]
    public List<VideoClip> videoClips;
    int clipNum = 0;
    bool isStopped = false;
    private VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        videoPlayer.clip = videoClips[0];
        videoPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStopped && !isPlaying())
        {
            Next();
            videoPlayer.Play();
        }
    }

    private void OnMouseDown()
    {
        if (isStopped) videoPlayer.Play();
        else videoPlayer.Pause();
        isStopped = !isStopped;
    }

    public void Back()
    {
        if (clipNum == 0) clipNum = videoClips.Count;
        clipNum--;
        videoPlayer.clip = videoClips[clipNum];
        isStopped = false;
    }

    public void Next()
    {

        clipNum++;
        if (clipNum == videoClips.Count) clipNum = 0;
        videoPlayer.clip = videoClips[clipNum];
        isStopped = false;
    }

    private bool isPlaying()
    {
        Debug.Log(videoPlayer.frame + " " + (long)videoPlayer.frameCount);
        return (videoPlayer.frame+1) != (long)videoPlayer.frameCount;
    }
}
