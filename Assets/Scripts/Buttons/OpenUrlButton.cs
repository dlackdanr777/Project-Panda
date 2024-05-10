using UnityEngine;
using UnityEngine.UI;

/// <summary> ��ư Ŭ���� �ش� URL�� �̵��ϰ� ���ִ� Ŭ���� </summary>
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
