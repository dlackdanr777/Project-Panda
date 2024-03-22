using UnityEngine;
using BT;
using Muks.DataBind;

public class MainScene : MonoBehaviour
{
    [SerializeField] private AudioClip _backgroundAudioClip;

    // Start is called before the first frame update

    private void Awake()
    {
        DataBind.SetButtonValue("ShopButton", OnShopButtonClicked);
    }

    void Start()
    {
        DataBind.SetTextValue("BambooCount", GameManager.Instance.Player.Bamboo.ToString());
        DatabaseManager.Instance.Challenges.CheckIsDone();
        StarterPanda.Instance.SwitchingScene();

        SoundManager.Instance.PlayBackgroundAudio(_backgroundAudioClip, 1);

     
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager.Instance.Player.GetMailList(Player.MailType.Mail).AddByStoryId("MS00C", MessageField.Mail);
        }
    }


    private void OnShopButtonClicked()
    {
        LoadingSceneManager.LoadScene("ShopScene");
    }



}
