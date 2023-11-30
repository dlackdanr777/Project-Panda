using Muks.DataBind;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInsideWood : UIView
{
    public event Action OnShowHandler;

    public event Action OnHideHandler;


    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        DataBind.SetButtonValue("WoodBorderButton", OnBorderButtonClicked);
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

    private void OnBorderButtonClicked()
    {
        _uiNav.Pop();
    }


}
