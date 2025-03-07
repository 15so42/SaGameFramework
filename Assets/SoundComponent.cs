using System;
using UnityEngine;
using System.Collections.Generic;

public enum AudioType
{
    BGM,
    SFX,
    UI_SFX
}

public class AudioSourceWrapper
{
    public AudioSource audioSource;
    public SoundDataRow soundDataRow;
}
public class SoundComponent : IGameComponent
{
   

    [Header("Volume Settings")]
    [Range(0, 1)] public float masterVolume = 1f;
    [Range(0, 1)] public float bgmVolume = 1f;
    [Range(0, 1)] public float sfxVolume = 1f;
    [Range(0, 1)] public float uiSfxVolume = 1f;

    private AudioSourceWrapper bgmSource;
    private readonly List<AudioSourceWrapper> activeSfxSources = new List<AudioSourceWrapper>();
    private readonly List<AudioSourceWrapper> activeUiSfxSources = new List<AudioSourceWrapper>();

    private const string POOL_NAME = "AudioSourcePool";
    private Transform audioPoolTrans;

    private SoundTable soundTable;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
        
        
    }

    private void Initialize()
    {
        // 创建BGM专用的AudioSource
        bgmSource = new AudioSourceWrapper
        {
            audioSource = gameObject.AddComponent<AudioSource>()
        };
        bgmSource.audioSource.loop = true;
        bgmSource.audioSource.playOnAwake = false;

        // 创建对象池
        audioPoolTrans = new GameObject(POOL_NAME).transform;
        audioPoolTrans.SetParent(transform);
    }

    private void Start()
    {
        //直接存储AudioTable
        soundTable = GameEntry.SoDataTable.GetTable<SoundDataRow>() as SoundTable;
    }

    public SoundDataRow GetSoundDataById(int id)
    {
        var dataRow = soundTable.GetDataRow(id) as SoundDataRow;
        if (dataRow == null)
        {
            throw new KeyNotFoundException($"获取Clip失败,没有找id为{id}的clip");
        }

        return dataRow;
    }

    public void PlayBGM(int id)
    {
        var soundDataRow = GetSoundDataById(id);
        bgmSource.soundDataRow = soundDataRow;
        PlayBGM(soundDataRow.audioClip);
    }

    private void PlayBGM(AudioClip clip)
    {
        if (bgmSource.audioSource.clip == clip && bgmSource.audioSource.isPlaying) return;

        bgmSource.audioSource.Stop();
        bgmSource.audioSource.clip = clip;
        bgmSource.audioSource.volume = GetFinalVolume(AudioType.BGM);//BGM不适用随机音量和pitch
        bgmSource.audioSource.Play();
    }

    public void PlaySFX3D(int id, Vector3 position, Transform followTarget=null)
    {
        var soundDataRow = GetSoundDataById(id);
        PlaySFX3D(soundDataRow,position,followTarget);
    }

    public AudioSourceWrapper PlaySFX3D(SoundDataRow soundDataRow, Vector3 position, Transform followTarget = null)
    {
        var source = GetAvailableSFXSource();
        Configure3DAudioSource(source.audioSource, position, followTarget);
        StartCoroutine(PlayClipAndReturnToPool(source.soundDataRow.audioClip,source.audioSource, AudioType.SFX,soundDataRow.pitch,soundDataRow.volume));
        return source;
    }
    
    public void PlaySFX2D(int id)
    {
        var soundDataRow = GetSoundDataById(id);
        PlaySFX2D(soundDataRow);
    }

    public AudioSourceWrapper PlaySFX2D(SoundDataRow soundDataRow)
    {
        var source = GetAvailableSFXSource();
        Configure2DAudioSource(source.audioSource);
        StartCoroutine(PlayClipAndReturnToPool(source.soundDataRow.audioClip,source.audioSource, AudioType.SFX,soundDataRow.pitch,soundDataRow.volume));
        return source;
    }
    

    public void PlayUISFX(int id)
    {
        var soundDataRow = GetSoundDataById(id);
        PlayUISFX(soundDataRow);
    }
    private AudioSourceWrapper PlayUISFX(SoundDataRow soundDataRow)
    {
        var source = GetAvailableUISFXSource();
        Configure2DAudioSource(source.audioSource);
        StartCoroutine(PlayClipAndReturnToPool(source.soundDataRow.audioClip,source.audioSource, AudioType.SFX,soundDataRow.pitch,soundDataRow.volume));
        return source;
    }

    private AudioSourceWrapper GetAvailableSFXSource()
    {
        return GetAvailableSourceFromPool("SFX", activeSfxSources);
    }

    private AudioSourceWrapper GetAvailableUISFXSource()
    {
        return GetAvailableSourceFromPool("UI_SFX", activeUiSfxSources);
    }

    private AudioSourceWrapper GetAvailableSourceFromPool(string prefix, List<AudioSourceWrapper> activeList)
    {
        var source = activeList.Find(s => !s.audioSource.isPlaying);
        if (source != null) return source;

        var newSource = new AudioSourceWrapper
        {
            audioSource = new GameObject($"{prefix}_AudioSource").AddComponent<AudioSource>()
        };

        newSource.audioSource.transform.SetParent(audioPoolTrans);
        activeList.Add(newSource);
        return newSource;
    }

    private void Configure3DAudioSource(AudioSource source, Vector3 position, Transform followTarget)
    {
        source.spatialBlend = 1f;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.minDistance = 1f;
        source.maxDistance = 50f;
        source.transform.position = position;

        if (followTarget != null)
        {
            source.transform.SetParent(followTarget);
            source.transform.localPosition = Vector3.zero;
        }
    }

    private void Configure2DAudioSource(AudioSource source)
    {
        source.spatialBlend = 0f;
        source.transform.SetParent(audioPoolTrans);
    }

    private System.Collections.IEnumerator PlayClipAndReturnToPool(AudioClip clip, AudioSource source, AudioType type,Vector2 pitchArea,Vector2 volumeArea)
    {
        source.clip = clip;
        source.volume = GetFinalVolume(type)* UnityEngine.Random.Range(volumeArea.x, volumeArea.y);
        source.pitch = UnityEngine.Random.Range(pitchArea.x, pitchArea.y);
        
        source.Play();

        yield return new WaitWhile(() => source.isPlaying);
        
        source.transform.SetParent(audioPoolTrans);
        source.transform.localPosition = Vector3.zero;
    }

    private float GetFinalVolume(AudioType type)
    {
        return type switch
        {
            AudioType.BGM => masterVolume * bgmVolume ,
            AudioType.SFX => masterVolume * sfxVolume,
            AudioType.UI_SFX => masterVolume * uiSfxVolume,
            _ => 1f
        };
    }

    public void UpdateVolumes()
    {
        bgmSource.audioSource.volume = GetFinalVolume(AudioType.BGM);
        
        foreach (var source in activeSfxSources)
            if (source.audioSource.isPlaying)
            {
                var originalMultiplier = source.audioSource.volume / GetFinalVolume(AudioType.SFX);
                source.audioSource.volume = GetFinalVolume(AudioType.SFX) * originalMultiplier;
            }
        
        foreach (var source in activeUiSfxSources)
            if (source.audioSource.isPlaying)
            {
                var originalMultiplier = source.audioSource.volume / GetFinalVolume(AudioType.UI_SFX);
                source.audioSource.volume = GetFinalVolume(AudioType.UI_SFX) * originalMultiplier;
            }
    }

    public void StopBgm()
    {
        bgmSource.audioSource.Stop();
    }

    public void StopAllSFX()
    {
        StopAllAudio(activeSfxSources);
    }

    public void StopAllUISFX()
    {
        StopAllAudio(activeUiSfxSources);
    }

    public void StopAllAudio()
    {
        StopBgm();
        StopAllSFX();
        StopAllUISFX();
    }

    private void StopAllAudio(List<AudioSourceWrapper> sources)
    {
        foreach (var source in sources)
        {
            source.audioSource.Stop();
            source.audioSource.transform.SetParent(audioPoolTrans);
        }
    }
}