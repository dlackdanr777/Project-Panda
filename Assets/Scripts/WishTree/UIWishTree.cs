using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWishTree : MonoBehaviour
{
    private UINavigation _uiNav;

    private string _value;

    private void Awake()
    {
        _uiNav = GetComponent<UINavigation>();

        DataBind.SetButtonValue("HouseButton", OnHouseButtonClicked);
        DataBind.SetButtonValue("InventoryButton", OnInventoryButtonClicked);
        DataBind.SetButtonValue("DiaryButton", OnDiaryButtonClicked);
        DataBind.SetButtonValue("BorderButton", OnBorderButtonClicked);
    }

    private void Start()
    {

    }

    private void OnHouseButtonClicked()
    {
        _uiNav.Push("House");
    }

    private void OnDiaryButtonClicked()
    {
        _uiNav.Push("Diary");
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
