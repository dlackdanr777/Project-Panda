using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;

public enum AudioType
{
    Master,
    BackgroundAudio,
    EffectAudio,
    Count,
}

public enum SoundEffectType
{
    ButtonClick,
    ButtonExit,
    Sell,
    Buy
}


public class SoundManager : SingletonHandler<SoundManager>
{
    [Header("Components")]
    [SerializeField] AudioMixer _audioMixer;

    [Space]
    [Header("Volume")]
    [Range(0f, 1f)]
    [SerializeField] private float _backgroundVolume;
    [Range(0f, 1f)]
    [SerializeField] private float _effectVolume;

    [Space]
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _buttonClick;
    [SerializeField] private AudioClip _buttonExit;
    [SerializeField] private AudioClip _sellSound;
    [SerializeField] private AudioClip _buySound;

    private AudioSource[] _audios;

    private float _backgroundVolumeMul;
    public float BackgroundVolumeMul => _backgroundVolumeMul;

    private float _effectVolumeMul;
    public float EffectVolumeMul => _effectVolumeMul;

    //��� ���� ����� ���� ��, �ٿ� ����� ���� ����
    private Coroutine _changeAudioRoutine;

    private Coroutine _stopBackgroundAudioRoutine;

    private Coroutine _stopEffectAudioRoutine;


    //�Ҹ� ������
    private float _saveMasterVolume;
    private float _saveBackgroundVolume;
    private float _saveSoundEffectVolume;

    //�븮��
    public event Action<float, AudioType> OnVolumeChangedHandler;

    public override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Start()
    {
        LoadSoundData();
    }


    private void Init()
    {
        _audios = new AudioSource[(int)AudioType.Count];

        for (int i = (int)AudioType.BackgroundAudio, count = (int)AudioType.Count; i < count; i++)
        {
            GameObject obj = new GameObject(Enum.GetName(typeof(AudioType), i));
            obj.transform.parent = transform;
            _audios[i] = obj.AddComponent<AudioSource>();
        }

        _backgroundVolumeMul = 1;
        _effectVolumeMul = 1;

        _audios[(int)AudioType.BackgroundAudio].loop = true;
        _audios[(int)AudioType.BackgroundAudio].playOnAwake = true;
        _audios[(int)AudioType.BackgroundAudio].volume = _backgroundVolume;
        _audios[(int)AudioType.BackgroundAudio].outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Master")[1];

        _audios[(int)AudioType.EffectAudio].loop = false;
        _audios[(int)AudioType.EffectAudio].playOnAwake = false;
        _audios[((int)AudioType.EffectAudio)].volume = _effectVolume;
        _audios[(int)AudioType.EffectAudio].outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Master")[2];

    }


    public float GetVolume(AudioType audioType)
    {
        switch (audioType)
        {
            case AudioType.Master:
                return _saveMasterVolume;

            case AudioType.BackgroundAudio:
                return _saveBackgroundVolume;

            case AudioType.EffectAudio:
                return _saveSoundEffectVolume;
        }

        return -1;
    }


    public void LoadSoundData()
    {
        float masterVolume = PlayerPrefs.HasKey("MasterVolume") ? PlayerPrefs.GetFloat("MasterVolume") : 0;
        _saveMasterVolume = masterVolume;
        _audioMixer.SetFloat("Master", _saveMasterVolume);

        float backgroundVolume = PlayerPrefs.HasKey("BackgroundVolume") ? PlayerPrefs.GetFloat("BackgroundVolume") : 0;
        _saveBackgroundVolume = backgroundVolume;
        _audioMixer.SetFloat("Background", _saveBackgroundVolume);

        float soundeffectVolume = PlayerPrefs.HasKey("SoundEffectVolume") ? PlayerPrefs.GetFloat("SoundEffectVolume") : 0;
        _saveSoundEffectVolume = soundeffectVolume;
        _audioMixer.SetFloat("SoundEffect", _saveSoundEffectVolume);
    }


    public void SaveSoundData(string name, float value)
    {
        PlayerPrefs.SetFloat(name + "Volume", value);
    }


    public void PlayBackgroundAudio(AudioClip clip, float duration = 0, bool isLoop = true)
    {
        if (_changeAudioRoutine != null)
            StopCoroutine(_changeAudioRoutine);
        _audios[(int)AudioType.BackgroundAudio].loop = isLoop;
        if (duration == 0)
        {
            _audios[(int)AudioType.BackgroundAudio].volume = _backgroundVolume;
            _audios[(int)AudioType.BackgroundAudio].clip = clip;
            _audios[(int)AudioType.BackgroundAudio].Play();
            return;
        }

        _changeAudioRoutine = StartCoroutine(IEChangeBackgroundAudio(clip, duration));
    }


    public void PlayEffectAudio(AudioClip clip)
    {
        _audios[(int)AudioType.EffectAudio].volume = _effectVolume;
        _audios[(int)AudioType.EffectAudio].PlayOneShot(clip);
    }


    public void PlayEffectAudio(SoundEffectType soundEffectType)
    {
        AudioClip clip = null;

        //����ġ���� ����� �ʿ��� Ŭ���� ã�� ������ �ִ´�.
        //(����� Ŭ���� �迭�� �־� ����ϸ� �ڵ尡 ���������� �������� �������Ƿ� ����ġ���� ����߽��ϴ�.)
        switch (soundEffectType)
        {
            case SoundEffectType.ButtonClick:
                clip = _buttonClick;
                break;

            case SoundEffectType.ButtonExit:
                clip = _buttonExit;
                break;

            case SoundEffectType.Sell:
                clip = _sellSound;
                break;

            case SoundEffectType.Buy:
                clip = _buySound;
                break;
        }

        _audios[(int)AudioType.EffectAudio].volume = _effectVolume;
        _audios[(int)AudioType.EffectAudio].PlayOneShot(clip);
    }



    public void StopBackgroundAudio(float duration = 0)
    {
        if(_stopBackgroundAudioRoutine != null)
            StopCoroutine(_stopBackgroundAudioRoutine);

        if (duration == 0)
        {
            _audios[(int)AudioType.BackgroundAudio].Stop();
            return;
        }

        _stopBackgroundAudioRoutine = StartCoroutine(IEStopBackgroundAudio(duration));
    }

    public void StopEffectAudio(float duration = 0)
    {
        if (_stopEffectAudioRoutine != null)
            StopCoroutine(_stopEffectAudioRoutine);

        if (duration == 0)
        {
            _audios[(int)AudioType.EffectAudio].Stop();
            return;
        }

        _stopEffectAudioRoutine = StartCoroutine(IEStopBackgroundAudio(duration));
    }



    public void SetVolume(float value, AudioType type)
    {
        float volume = value != 0 ? Mathf.Log10(value) * 20 : -80;

        switch (type)
        {
            case AudioType.Master:
                _audioMixer.SetFloat("Master", volume);
                SaveSoundData("Master", volume);
                break;

            case AudioType.BackgroundAudio:
                _audioMixer.SetFloat("Background", volume);
                SaveSoundData("Background", volume);
                break;

            case AudioType.EffectAudio:
                _audioMixer.SetFloat("SoundEffect", volume);
                SaveSoundData("SoundEffect", volume);
                break;
        }


        OnVolumeChangedHandler?.Invoke(volume, type);
    }


    private IEnumerator IEStopBackgroundAudio(float duration)
    {
        float maxVolume = _audios[(int)AudioType.BackgroundAudio].volume;
        float changeDuration = duration;
        float timer = 0;

        while (timer < changeDuration)
        {
            timer += 0.02f;
            _audios[(int)AudioType.BackgroundAudio].volume = Mathf.Lerp(maxVolume, 0, timer / changeDuration);

            yield return YieldCache.WaitForSeconds(0.02f);
        }

        _audios[(int)AudioType.BackgroundAudio].Stop();
    }


    private IEnumerator IEChangeBackgroundAudio(AudioClip clip, float duration)
    {
        float maxVolume = _backgroundVolume;
        float changeDuration = duration * 0.5f;
        float timer = 0;

        while (timer < changeDuration)
        {
            timer += 0.02f;
            _audios[(int)AudioType.BackgroundAudio].volume = Mathf.Lerp(maxVolume, 0, timer / changeDuration);

            yield return YieldCache.WaitForSeconds(0.02f);
        }

        _audios[(int)AudioType.BackgroundAudio].clip = clip;
        _audios[(int)AudioType.BackgroundAudio].volume = 0;
        _audios[(int)AudioType.BackgroundAudio].Play();

        timer = 0;
        while (timer < changeDuration)
        {
            timer += 0.02f;
            _audios[(int)AudioType.BackgroundAudio].volume = Mathf.Lerp(0, maxVolume, timer / changeDuration);

            yield return YieldCache.WaitForSeconds(0.02f);
        }
    }


    private IEnumerator IEStopEffectAudio(float duration)
    {
        float maxVolume = _audios[(int)AudioType.EffectAudio].volume;
        float changeDuration = duration;
        float timer = 0;

        while (timer < changeDuration)
        {
            timer += 0.02f;
            _audios[(int)AudioType.EffectAudio].volume = Mathf.Lerp(maxVolume, 0, timer / changeDuration);

            yield return YieldCache.WaitForSeconds(0.02f);
        }

        _audios[(int)AudioType.BackgroundAudio].Stop();
    }
}

