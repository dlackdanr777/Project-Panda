using UnityEngine;
using UnityEngine.UI;

/// <summary> 버튼 클릭시 해당 URL로 이동하게 해주는 클래스 </summary>
public class OpenUrlButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private string _openURL;


    void Start()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }


    private void OnButtonClicked()
    {
        if (string.IsNullOrWhiteSpace(_openURL))
            return;

        Application.OpenURL(_openURL);
    }
}
