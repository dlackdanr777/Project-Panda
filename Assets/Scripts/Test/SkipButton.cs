using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkipButton : MonoBehaviour
{
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnSkipButtonClicked);
    }

    private void OnSkipButtonClicked()
    {
        LoadingSceneManager.LoadScene("24_01_09_Integrated");
        _button.onClick.RemoveListener(OnSkipButtonClicked);
    }
}
