using UnityEngine;
using UnityEngine.UI;

/// <summary>환경설정에서 옵션을 조절할 수 있게 해주는 클래스</summary>
public class UIPreferences : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Slider _backgroundSlider;
    [SerializeField] private Slider _soundEffectSlider;

    [SerializeField] private AudioClip _clip;

    public void Init()
    {
        _backgroundSlider.onValueChanged.AddListener(BackgroundSliderValueChanged);
        _soundEffectSlider.onValueChanged.AddListener(SoundEffectSliderValueChanged);

        SoundManager.Instance.PlayBackgroundAudio(_clip);
    }


    private void BackgroundSliderValueChanged(float value)
    {
        SoundManager.Instance.SetVolume(value, AudioType.BackgroundAudio);
    }


    private void SoundEffectSliderValueChanged(float value)
    {
        SoundManager.Instance.SetVolume(value, AudioType.EffectAudio);
    }
}
