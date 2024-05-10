using Muks.BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIBugReport : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private UIBugReportPopup _popup;
    [SerializeField] private Button _okButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private TMP_InputField _inputField;


    public void Init(UnityAction cancelButtonClicked)
    {
        _okButton.onClick.AddListener(OkButtonClicked);
        _cancelButton.onClick.AddListener(cancelButtonClicked);
        _popup.Init(cancelButtonClicked);
        _popup.gameObject.SetActive(false);
    }


    public void Show()
    {
        _popup.Hide();
    }


    public void Hide()
    {
        _popup.Hide();
        _inputField.text = string.Empty;
    }


    private void OkButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
        BackendManager.Instance.BugReportUpload(_inputField.text);
        _popup.Show("소중한 제보 감사드립니다.");
    }

}
