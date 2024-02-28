using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;


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
    public List<AudioClip> sfxClips;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;
    public Slider SFXVolumeSlider;


    // public enum AllSfx { Appear, Jump, Selling, Portal, Select, Punch1,
    // ShieldAtk, ShieldJumpAtk, SwordUpperAtk, SwordWidthAtk, BowAtk = 11, BowCharging, Walk
    // , SouthSkill, WaterBomb, Inventory, fallDownAtk };


    public enum Sfx
    {
        Walk, Jump, Dash, Hit, // 기본 행동 4가지
        Sword_WidthAtk, Sword_UpperAtk, Sword_JumpAtk, Sword_SkillStart, Sword_Skill, // 칼 행동 5가지
        Wand_Atk, Wand_Skill, Wand_Bomb, // 완드 행동 3가지
        Shield_Charging, Shield_Atk, Shield_JumpAtk, Shield_Skill, // 방패 행동 4가지
        Bow_Charging, Bow_Shoot, Bow_SkillCharging, Bow_SkillShoot, // 활 행동 4가지
        Inventory_Open, Shop_Open, Select, ItemBuy, ItemSell, ItemChange, Portal, // UI 행동 7가지
        Wolf_Dash, Wolf_Bite, Wolf_Hand, // 늑대 행동 3가지
        Bear_HandOb, Bear_HandDown, Bear_RushStart, Bear_RushMove, Bear_JumpDown, Bear_Appear // 곰 행동 6가지
    }



    
    [ListDrawerSettings(ShowIndexLabels = true)]
    [System.Serializable] public class SoundData
    {
        [LabelText("사운드 기능")] public String soundName;
        [LabelText("사운드 클립")] public AudioClip soundClip;
    }

    [ListDrawerSettings(ShowIndexLabels = true)]
    [System.Serializable] public class SoundDataList
    {
        [LabelText("상위 정보")] public String infoName;
        public List<SoundData> soundDatas;
    }

    
    [TitleGroup("캐릭터 기본 사운드")]
    [LabelText("캐릭터 기본 사운드")] public List<SoundData> normalSoundDatas;

    
    [TitleGroup("캐릭터 무기별 사운드")]
    [LabelText("각 무기별 사운드")] public List<SoundDataList> weaponSoundDatas;

    
    [TitleGroup("UI 사운드")] 
    [LabelText("UI 별 사운드")] public List<SoundData> uiSoundDatas;


    [TitleGroup("몬스터별 효과음")]
    [LabelText("몬스터 별 별 사운드")] public List<SoundDataList> monsterSoundDatas;


    public void SetAudioClips()
    {
        sfxClips = new List<AudioClip>();

        for (int i = 0; i < normalSoundDatas.Count; i++)
            sfxClips.Add(normalSoundDatas[i].soundClip);

        for (int i = 0; i < weaponSoundDatas.Count; i++)
        {
            for (int j = 0; j < weaponSoundDatas[i].soundDatas.Count; j++)
            {
                sfxClips.Add(weaponSoundDatas[i].soundDatas[j].soundClip);
            }
        }
            
        for (int i = 0; i < uiSoundDatas.Count; i++)
            sfxClips.Add(uiSoundDatas[i].soundClip);

        for (int i = 0; i < monsterSoundDatas.Count; i++)
        {
            for (int j = 0; j < monsterSoundDatas[i].soundDatas.Count; j++)
            {
                sfxClips.Add(monsterSoundDatas[i].soundDatas[j].soundClip);
            }
        }
    }

    
    


    void Awake()
    {
        if(!instance)
            instance = this;
        else
        {
            instance.Set();
            Destroy(gameObject);
        }
        

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        SetAudioClips();
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
                // case Sfx.SwordAtk:
                //     randIndex += Random.Range(0, 2 + 1);
                //     break;
                
                // case Sfx.fallDownAtk:
                //     randIndex += UnityEngine.Random.Range(0, 1 + 1);
                //     break;

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
        PlayerPrefs.SetFloat("BGMData", BGMVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXData", SFXVolumeSlider.value);
    }

    public void SoundLoad()
    {
        if(PlayerPrefs.HasKey("BGMData") && BGMVolumeSlider != null)
            BGMVolumeSlider.value = PlayerPrefs.GetFloat("BGMData");
        else if(!PlayerPrefs.HasKey("BGMData"))
            Debug.Log("Dont has Ket BGMData");
        else
            Debug.Log("Null Reference 1");
            
            
        if(PlayerPrefs.HasKey("SFXData") && BGMVolumeSlider != null)
            SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXData");
        else if(!PlayerPrefs.HasKey("SFXData"))
            Debug.Log("Dont has Ket SFXData");
        else
            Debug.Log("Null Reference 2");
    }
}
