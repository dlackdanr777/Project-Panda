using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>환경설정에서 옵션을 조절할 수 있게 해주는 클래스</summary>
public class UIPreferences : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button _silentButton;
    [SerializeField] private Button _vibrateButton;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Slider _backgroundSlider;
    [SerializeField] private Slider _soundEffectSlider;


    public void Init()
    {
        float backgroundVolume = SoundManager.Instance.GetVolume(AudioType.BackgroundAudio);
        _backgroundSlider.value = backgroundVolume != -80 ? Mathf.Pow(10, backgroundVolume / 20) : 0;

        float soundEffectVolume = SoundManager.Instance.GetVolume(AudioType.EffectAudio);
        _soundEffectSlider.value = soundEffectVolume != -80 ? Mathf.Pow(10, soundEffectVolume / 20) : 0;

        _silentButton.onClick.AddListener(OnSilentButtonClicked);
        _vibrateButton.onClick.AddListener(OnVibrateButtonClicked);
        _soundButton.onClick.AddListener(OnSoundButtonClicked);
        _backgroundSlider.onValueChanged.AddListener(OnBackgroundSliderValueChanged);
        _soundEffectSlider.onValueChanged.AddListener(OnSoundEffectSliderValueChanged);
    }


    private void OnSilentButtonClicked()
    {
        SoundManager.Instance.SetVolume(0, AudioType.BackgroundAudio);
        SoundManager.Instance.SetVolume(0, AudioType.EffectAudio);

        _backgroundSlider.value = 0;
        _soundEffectSlider.value = 0;

        //TODO: 진동기능 끄기
    }


    private void OnVibrateButtonClicked()
    {
        SoundManager.Instance.SetVolume(0, AudioType.BackgroundAudio);
        SoundManager.Instance.SetVolume(0, AudioType.EffectAudio);

        _backgroundSlider.value = 0;
        _soundEffectSlider.value = 0;

        //TODO: 진동기능 켜기
    }


    private void OnSoundButtonClicked()
    {
        SoundManager.Instance.SetVolume(1, AudioType.BackgroundAudio);
        SoundManager.Instance.SetVolume(1, AudioType.EffectAudio);

        _backgroundSlider.value = 1;
        _soundEffectSlider.value = 1;
    }


    private void OnBackgroundSliderValueChanged(float value)
    {
        SoundManager.Instance.SetVolume(value, AudioType.BackgroundAudio);
    }


    private void OnSoundEffectSliderValueChanged(float value)
    {
        SoundManager.Instance.SetVolume(value, AudioType.EffectAudio);
    }
}
