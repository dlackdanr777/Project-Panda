using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleBtnClick : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    private Button _button;

    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClickToggleButton);
    }

    private void OnClickToggleButton()
    {
        _panel.SetActive(!_panel.activeSelf);
    }
}
