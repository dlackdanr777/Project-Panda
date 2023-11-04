using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UINavigation))]
public class UIPhone : MonoBehaviour
{
    private UINavigation _uiNav;

    private void Awake()
    {
        _uiNav = GetComponent<UINavigation>();

        DataBinding.SetButtonValue("PhoneButton", OnPhoneButtonClicked);
        DataBinding.SetButtonValue("CameraButton", OnCameraButtonClicked);
        DataBinding.SetButtonValue("LibaryButton", OnLibaryButtonClicked);
        DataBinding.SetButtonValue("BorderButton", OnBorderButtonClicked);
    }


    private void OnPhoneButtonClicked()
    { 
        _uiNav.Clear();
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void OnCameraButtonClicked()
    {
        _uiNav.Push("Camera");
    }

    private void OnLibaryButtonClicked()
    {
        _uiNav.Push("Album");
    }

    private void OnBorderButtonClicked()
    {
        _uiNav.Pop();
    }
}
