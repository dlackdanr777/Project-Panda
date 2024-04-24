using UnityEngine;

public class ShopScene : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _backgroundSound;

    void Start()
    {
        SoundManager.Instance.PlayBackgroundAudio(_backgroundSound, 1);

        Muks.DataBind.DataBind.SetTextValue("BambooCount", GameManager.Instance.Player.Bamboo.ToString());
    }

}
