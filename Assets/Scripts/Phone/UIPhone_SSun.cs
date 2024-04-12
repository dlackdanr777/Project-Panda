using System;
using UnityEngine;
using Muks.DataBind;

[RequireComponent(typeof(UINavigation))]
public class UIPhone_Ssun : MonoBehaviour
{
    private UINavigation _uiNav;

    private string _value;

    private void Awake()
    {
        _uiNav = GetComponent<UINavigation>();

        DataBind.SetUnityActionValue("PhoneButton", OnPhoneButtonClicked);
        DataBind.SetUnityActionValue("InventoryButton", OnInventoryButtonClicked);
        DataBind.SetUnityActionValue("ItemButton", OnItemButtonClicked);
        DataBind.SetUnityActionValue("ShopButton", OnShopButtonClicked);
        DataBind.SetUnityActionValue("BorderButton", OnBorderButtonClicked);
    }

    private void Start()
    {

    }

    private void OnPhoneButtonClicked()
    {
        _uiNav.Clear();
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void OnItemButtonClicked()
    {
        _uiNav.Push("Item");
    }
    private void OnInventoryButtonClicked()
    {
        _uiNav.Push("Inventory");
    }

    private void OnShopButtonClicked()
    {
        _uiNav.Push("Shop");
    }

    private void OnBorderButtonClicked()
    {
        _uiNav.Pop();
    }
}
