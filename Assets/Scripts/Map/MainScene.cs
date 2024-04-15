using BT;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    [SerializeField] private AudioClip _backgroundAudioClip;

    void Start()
    {
        DatabaseManager.Instance.Challenges.CheckIsDone();
        StarterPanda.Instance.SwitchingScene();
        SoundManager.Instance.PlayBackgroundAudio(_backgroundAudioClip, 1);
    }
}
