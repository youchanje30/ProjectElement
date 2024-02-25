using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private UIController uiCon;
   
    
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


    public enum Sfx { Appear, Jump, Selling, Portal, Select, Punch1,
    ShieldAtk, ShieldJumpAtk, SwordAtk, BowAtk = 11, BowCharging, Walk
    , SouthSkill, WaterBomb, Inventory, fallDownAtk };


    void Awake()
    {
        if(!instance)
            instance = this;
        else
        {
            instance.Set();
            Destroy(gameObject);
        }
        

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {    
        Set();
        Init();
    }

    public void Set()
    {
        uiCon = GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>();
        
        if(!BGMVolumeSlider)
            BGMVolumeSlider = uiCon.bgmSlider;

        if(!SFXVolumeSlider)
            SFXVolumeSlider = uiCon.sfxSlider;

        Invoke("SoundLoad", 0.1f);
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

            int randIndex = 0;
            switch (sfx)
            {
                case Sfx.SwordAtk:
                    randIndex += Random.Range(0, 2 + 1);
                    break;
                
                case Sfx.fallDownAtk:
                    randIndex += Random.Range(0, 1 + 1);
                    break;

                default:
                    break;
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + randIndex];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void StopSfx(Sfx sfx)
    {
        for (int i = 0; i < sfxPlayers.Length; i++)
        {

            if(sfxPlayers[i].clip == sfxClips[(int)sfx] && sfxPlayers[i].isPlaying)
            {
                sfxPlayers[i].Stop();
                break;
            }
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

    void AutoSoundSave()
    {
        SoundSave();
        Invoke("AutoSoundSave", 10f);
    }


    public void SoundSave()
    {
        PlayerPrefs.SetFloat("BGMVol", BGMVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVol", SFXVolumeSlider.value);
    }

    public void SoundLoad()
    {
        if(PlayerPrefs.HasKey("BGMVol") && BGMVolumeSlider != null)
            BGMVolumeSlider.value = PlayerPrefs.GetFloat("BGMVol");
            
        if(PlayerPrefs.HasKey("SFXVol") && BGMVolumeSlider != null)
            SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVol");
    }
}
