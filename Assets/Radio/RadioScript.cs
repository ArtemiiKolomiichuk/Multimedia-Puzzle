using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RadioScript : MonoBehaviour
{
    [SerializeField]
    public List<AudioClip> audioClips;
    int clipNum = 0;
    bool isStopped = true;
    private AudioSource audioSource;
    [SerializeField]
    GameObject text;
    private TextMeshProUGUI textMeshPro;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        textMeshPro = text.GetComponent<TextMeshProUGUI>();
        if (audioClips.Count > 0)
        {
            audioSource.clip = audioClips[0];
            resetName();
        }
            
        audioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStopped && !isPlaying() && audioClips.Count > 0)
        {
            isStopped = true;
        }
    }

    public void StopOrPlay()
    {
        if (audioClips.Count > 0)
        {
            if (isStopped)
            {
                if (!isPlaying())
                {
                    audioSource.Stop();
                }
                audioSource.Play();
            }
            else
            {
                audioSource.Pause();
            }
            isStopped = !isStopped;
        }
    }


    public void Back()
    {
        if (audioClips.Count > 0)
        {
            if (clipNum == 0) clipNum = audioClips.Count;
            clipNum--;
            audioSource.clip = audioClips[clipNum];
            isStopped = false;
            resetName();
        }
    }

    public void Next()
    {
        if (audioClips.Count > 0)
        {
            clipNum++;
            if (clipNum == audioClips.Count) clipNum = 0;
            audioSource.clip = audioClips[clipNum];
            isStopped = false;
            resetName();
        }
    }

    private void resetName()
    {
        textMeshPro.text = audioSource.clip.name;
    }

    private bool isPlaying()
    {
        return audioSource.isPlaying;
    }
}
