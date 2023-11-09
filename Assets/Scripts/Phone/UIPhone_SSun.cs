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

        DataBind.SetButtonValue("InventoryButton", OnInventoryButtonClicked);
        DataBind.SetButtonValue("BorderButton", OnBorderButtonClicked);
    }

    private void Start()
    {

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
