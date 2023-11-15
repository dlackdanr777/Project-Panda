using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UINavigation))]
public class UIPhone_DK : MonoBehaviour
{
    private UINavigation _uiNav;

    private string _value;

    private void Awake()
    {
        _uiNav = GetComponent<UINavigation>();

        DataBind.SetButtonValue("ShopButton", OnShopButtonClicked);
        DataBind.SetButtonValue("BorderButton", OnBorderButtonClicked);
    }

    private void Start()
    {

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
