using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RadioScript : MonoBehaviour
{
    [SerializeField]
    public List<AudioClip> audioClips;
    [SerializeField]
    [TextArea(3, 10)]
    internal List<string> songTitles;
    public int clipNum = 0;
    public bool isStopped = true;
    private AudioSource audioSource;
    [SerializeField]
    GameObject text;
    private TextMeshProUGUI textMeshPro;
    [SerializeField]
    public int goodSong;
    [SerializeField]
    public int badSong;
    [SerializeField]
    public GameObject whiteNoiseObject;
    private AudioSource whiteNoise;
    void Start()
    {
        CustomVolumeManager.onVolumeChange.AddListener(ChangeVolume);
        audioSource = GetComponent<AudioSource>();
        if(text!=null) textMeshPro = text.GetComponent<TextMeshProUGUI>();
        if (audioClips.Count > 0)
        {
            audioSource.clip = audioClips[0];
            resetName();
        }
            
        audioSource.Stop();
        whiteNoise = whiteNoiseObject.GetComponent<AudioSource>();
        VolumeManagerChangeState();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStopped && !isPlaying() && audioClips.Count > 0)
        {
            isStopped = true;
            audioSource.Pause();
            whiteNoise.volume = 0;
        }
    }

    private void ChangeVolume(float? x)
    {
        if (clipNum == goodSong || clipNum == badSong)
        {
            if (x == null)
            {
                audioSource.volume = 0;
                if (!isStopped) whiteNoise.volume = 0.1f;
                else whiteNoise.volume = 0;
            }
            else
            {
                audioSource.volume = (float)x;
                if (!isStopped) whiteNoise.volume = 1.2f*(1 - (float)x)/10;
                else whiteNoise.volume = 0;
            }
        }
        else
        {
            audioSource.volume = 1;
            whiteNoise.volume = 0;
        }
    }

    public void StopOrPlay()
    {
        if (audioClips.Count > 0)
        {
            if (isStopped)
            {
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
            isStopped = true;
            whiteNoise.volume = 0;
            resetName();
        }
        StopOrPlay();
        VolumeManagerChangeState();
        if (CustomVolumeManager.Instance.state == CustomVolumeManager.ManagerState.Off) audioSource.volume = 1;
    }

    public void Next()
    {
        if (audioClips.Count > 0)
        {
            clipNum++;
            if (clipNum == audioClips.Count) clipNum = 0;
            audioSource.clip = audioClips[clipNum];
            isStopped = true;
            whiteNoise.volume = 0;
            resetName();
        }
        StopOrPlay();
        VolumeManagerChangeState();
        if (CustomVolumeManager.Instance.state == CustomVolumeManager.ManagerState.Off) audioSource.volume = 1;
    }

    public void VolumeManagerChangeState()
    {
        if (clipNum == goodSong) CustomVolumeManager.Instance.state = CustomVolumeManager.ManagerState.Good;
        else if (clipNum == badSong) CustomVolumeManager.Instance.state = CustomVolumeManager.ManagerState.Bad;
        else CustomVolumeManager.Instance.state = CustomVolumeManager.ManagerState.Off;
    }
    private void resetName()
    {
        if(textMeshPro!=null) textMeshPro.text = songTitles[clipNum];
    }

    private bool isPlaying()
    {
        return audioSource.isPlaying;
    }
}
