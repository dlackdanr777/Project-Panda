using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UIAttendanceButton : MonoBehaviour
{

    [SerializeField] private GameObject _dontTouchArea;

    [SerializeField] private Button _button;


    public void EnableButtonClick(UnityAction _onClicked)
    {
        _dontTouchArea.SetActive(false);
        _button.onClick.AddListener(_onClicked);
        _button.onClick.AddListener(DisableButtonClick);
    }


    public void DisableButtonClick()
    {
        _dontTouchArea.SetActive(true);
        _button.onClick.RemoveAllListeners();
    }
}
