using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferencesData
{
    //Audio Option
    private float _masterVolume;
    private float _backgroundVolume;
    private float _soundEffectVolume;


    public float GetVolume(AudioType type)
    {
        switch (type)
        {
            case AudioType.Master:
                return _masterVolume;

            case AudioType.BackgroundAudio:
                return _backgroundVolume;

            case AudioType.EffectAudio:
                return _soundEffectVolume;
        }

        return -1;
    }


    public void SetVolume(AudioType type, float value)
    {
        switch (type)
        {
            case AudioType.Master:
                _masterVolume = value;
                break;

            case AudioType.BackgroundAudio:
                _backgroundVolume = value;
                break;

            case AudioType.EffectAudio:
                _soundEffectVolume = value;
                break;
        }
    }
}
