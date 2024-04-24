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

        Muks.DataBind.DataBind.SetTextValue("BambooCount", GameManager.Instance.Player.Bamboo.ToString());

/*        for(int i = 1; i <= 20; i++)
        {
            GameManager.Instance.Player.AddItemById("IBG0" + i);
            GameManager.Instance.Player.AddItemById("IFI0" + i);
            GameManager.Instance.Player.AddItemById("IFR0" + i);
            GameManager.Instance.Player.AddItemById("CookFd0" + i);
        }*/

    }
}
