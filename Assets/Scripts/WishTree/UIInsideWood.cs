using Muks.DataBind;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInsideWood : UIView
{
    [SerializeField] private GameObject _borderButton;

    public event Action OnShowHandler;

    public event Action OnHideHandler;

    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        DataBind.SetButtonValue("WoodBorderButton", OnBorderButtonClicked);
        DataBind.SetButtonValue("InventoryButton", OnInventoryButtonClicked);
        DataBind.SetButtonValue("DiaryButton", OnDiaryButtonClicked);
    }

    public override void Show()
    {
        OnShowHandler?.Invoke();
        gameObject.SetActive(true);
    }
    
    public override void Hide()
    {
        OnHideHandler?.Invoke();
        gameObject.SetActive(false);
    }

    private void OnDiaryButtonClicked()
    {
        if (!_uiNav.Check("Diary"))
        {
            _uiNav.Push("Diary");
        }
        else
        {
            _uiNav.Pop("Diary");
        }
    }


    private void OnInventoryButtonClicked()
    {
        if (!_uiNav.Check("WishTreeInventory"))
        {
            _uiNav.Push("WishTreeInventory");
        }
        else
        {
            _uiNav.Pop("WishTreeInventory");
        }       
    }


    private void OnBorderButtonClicked()
    {
        _uiNav.Pop();
    }


}
