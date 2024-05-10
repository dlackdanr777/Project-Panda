using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIBugReportPopup : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button _okButton;
    [SerializeField] private TextMeshProUGUI _titleText;

    public void Init(UnityAction okButtonClicked)
    {
        _okButton.onClick.AddListener(okButtonClicked);
        _okButton.onClick.AddListener(Hide);
    }


    public void Show(string title)
    {
        _titleText.text = title;
        gameObject.SetActive(true);
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
