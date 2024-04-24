using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookScene : MonoBehaviour
{
    [SerializeField] private AudioClip _backgroundAudiolClip;

    void Start()
    {
        SoundManager.Instance.PlayBackgroundAudio(_backgroundAudiolClip, 1);
    }
}
