using UnityEngine;
using UnityEngine.UI;

public class ChangeCookSceneButton : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button _button;

    void Start()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }


    private void OnButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

        if(DatabaseManager.Instance.UserInfo.IsCookTutorialClear)
            LoadingSceneManager.LoadScene("CookingScene");

        else
            LoadingSceneManager.LoadScene("CookingTutorialScene");
    }
}
