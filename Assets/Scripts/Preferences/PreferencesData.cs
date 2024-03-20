using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferencesData
{
    //Audio Option
    private float _masterVolume;
    private float _backgroundVolume;
    private float _soundEffectVolume;

    public void SetMasterVolume(float value)
    {
        _masterVolume = value;
    }

    public void SetBackgroundVolume(float value)
    {
        _backgroundVolume = value;
    }

    public void SetSoundEffectVolume(float value)
    {
        _soundEffectVolume = value;
    }
}
