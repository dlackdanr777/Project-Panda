using Muks.DataBind;
using UnityEngine;

public class ShopScene : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _backgroundSound;


    private void Awake()
    {
        DataBind.SetTextValue("BambooCount", GameManager.Instance.Player.Bamboo.ToString());
    }


    void Start()
    {
        SoundManager.Instance.PlayBackgroundAudio(_backgroundSound, 1);
    }

}
