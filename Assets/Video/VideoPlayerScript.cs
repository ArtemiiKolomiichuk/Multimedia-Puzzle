using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerScript : MonoBehaviour
{
    [SerializeField]
    public List<VideoClip> videoClips;
    int clipNum = 0;
    bool isStopped = true;
    private VideoPlayer videoPlayer;
    private MeshRenderer meshRenderer;
    [SerializeField]
    Material play;
    [SerializeField]
    Material replay;
    void Start()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        if(videoClips.Count > 0)
        videoPlayer.clip = videoClips[0];
        meshRenderer = transform.Find("play").GetComponent<MeshRenderer>();
        videoPlayer.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStopped && !isPlaying() && videoClips.Count > 0)
        {
            isStopped = true;
            meshRenderer.material = replay;
            meshRenderer.enabled = true;
        }
    }

    private void OnMouseDown()
    {
        if (videoClips.Count > 0)
        {
            if (isStopped)
            {
                if (!isPlaying())
                {
                    videoPlayer.Stop();
                }
                videoPlayer.Play();
            }
            else
            {
                meshRenderer.material = play;
                videoPlayer.Pause();
            }
            isStopped = !isStopped;
            meshRenderer.enabled = !meshRenderer.enabled;
        }
    }

    
    public void Back()
    {
        if (videoClips.Count > 0)
        {
            if (clipNum == 0) clipNum = videoClips.Count;
            clipNum--;
            videoPlayer.clip = videoClips[clipNum];
            meshRenderer.enabled = isStopped = false;
        }
    }

    public void Next()
    {
        if (videoClips.Count > 0)
        {
            clipNum++;
            if (clipNum == videoClips.Count) clipNum = 0;
            videoPlayer.clip = videoClips[clipNum];
            meshRenderer.enabled = isStopped = false;
        }
    }

    private bool isPlaying()
    {
        return (videoPlayer.frame+1) != (long)videoPlayer.frameCount;
    }
}
