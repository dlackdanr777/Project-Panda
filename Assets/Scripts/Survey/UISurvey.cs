using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISurvey : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button _okButton;
    [SerializeField] private Button _cancelButton;


    private string _openURL = "https://form.naver.com/response/qPvOLlZ53DI_HWJEKcazTQ";


    public void Init(UnityAction cancelButtonClicked)
    {
        _okButton.onClick.AddListener(OkButtonClicked);
        _cancelButton.onClick.AddListener(cancelButtonClicked);

    }


    private void OkButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
        DatabaseManager.Instance.UserInfo.IsSurveyButtonClicked = true;
        Application.OpenURL(_openURL);
    }

}
