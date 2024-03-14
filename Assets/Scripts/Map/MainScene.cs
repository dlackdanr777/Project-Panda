using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;
using Muks.DataBind;

public class MainScene : MonoBehaviour
{
    [SerializeField] private AudioClip _backgroundAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        DataBind.SetTextValue("BambooCount", GameManager.Instance.Player.Bamboo.ToString());
        DatabaseManager.Instance.Challenges.CheckIsDone();
        StarterPanda.Instance.SwitchingScene();

        SoundManager.Instance.PlayBackgroundAudio(_backgroundAudioClip, 1);
    }
}
