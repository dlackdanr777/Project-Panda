using UnityEngine;

public class ShopScene : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _backgroundSound;

    void Start()
    {
        SoundManager.Instance.PlayBackgroundAudio(_backgroundSound, 1);
    }

}
