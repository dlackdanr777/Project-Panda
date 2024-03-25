using UnityEngine;
using UnityEngine.UI;

public class ChangeSceneButton : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button _button;

    [Space]
    [Header("Settings")]
    [SerializeField] private string _changeSceneName;


    void Start()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }


    private void OnButtonClicked()
    {
        LoadingSceneManager.LoadScene(_changeSceneName);
    }
}