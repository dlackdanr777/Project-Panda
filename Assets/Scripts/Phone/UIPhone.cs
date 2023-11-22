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
        DataBind.SetButtonValue("BorderButton", OnBorderButtonClicked);
        DataBind.SetButtonValue("CameraButton", OnCameraButtonClicked);
        DataBind.SetButtonValue("LibaryButton", OnLibaryButtonClicked);
        DataBind.SetButtonValue("InventoryButton", OnInventoryButtonClicked);
        DataBind.SetButtonValue("ItemButton", OnItemButtonClicked);
    }

    private void Start()
    {
        Invoke("Hide", 0.02f);
    }

    private void Hide() => gameObject.SetActive(false);


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

    private void OnItemButtonClicked()
    {
        _uiNav.Push("Item");
    }
    private void OnInventoryButtonClicked()
    {
        _uiNav.Push("Inventory");
    }

    private void OnBorderButtonClicked()
    {
        _uiNav.Pop();
    }
}
