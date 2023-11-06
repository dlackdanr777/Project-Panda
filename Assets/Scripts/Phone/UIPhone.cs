using System;
using UnityEngine;
using Muks.DataBind;

[RequireComponent(typeof(UINavigation))]
public class UIPhone : MonoBehaviour
{
    private UINavigation _uiNav;

    private string _value;

    private void Awake()
    {
        _uiNav = GetComponent<UINavigation>();

        DataBind.SetButtonValue("PhoneButton", OnPhoneButtonClicked);
        DataBind.SetButtonValue("CameraButton", OnCameraButtonClicked);
        DataBind.SetButtonValue("LibaryButton", OnLibaryButtonClicked);
        DataBind.SetButtonValue("BorderButton", OnBorderButtonClicked);
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
        _uiNav.Push("Library");
    }

    private void OnBorderButtonClicked()
    {
        _uiNav.Pop();
    }
}
