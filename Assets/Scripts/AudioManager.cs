using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

   
    
    [Header("BGM 설정")]
    public AudioClip bgmClip;
    AudioSource bgmPlayer;
    public Slider BGMVolumeSlider;
    [Space(20f)]


    [Header("SFX 설정")]
    public AudioClip[] sfxClips;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;
    public Slider SFXVolumeSlider;


    public enum Sfx {};


    void Awake()
    {
        instance = this;
        
    }

    void Start()
    {
        if(!BGMVolumeSlider)
            BGMVolumeSlider = UIController.instance.bgmSlider;

        if(!SFXVolumeSlider)
            SFXVolumeSlider = UIController.instance.sfxSlider;

        Init();
    }

    void Update()
    {
    }

    void Init()
    {
        //BGM Player 초기화
        GameObject bgmObject = new GameObject("BGMPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = BGMVolumeSlider.value;
        bgmPlayer.clip = bgmClip;


        //SFX Player 초기화
        GameObject sfxObject = new GameObject("SFXPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];
        for(int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = SFXVolumeSlider.value;
        }

    }
    

    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if(sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if(isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }

    }

    public void ToggleBGM()
    {
        bgmPlayer.mute = !bgmPlayer.mute;
    }

    public void ToggleSFX()
    {
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i].mute = !sfxPlayers[i].mute;
        }
    }

    public void ChangeBGMVol()
    {
        bgmPlayer.volume = BGMVolumeSlider.value;
    }

    public void ChangeSFXVol()
    {
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i].volume = SFXVolumeSlider.value;
        }
    }

}
