using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInsideWood : UIView
{
    public event Action OnShowHandler;

    public event Action OnHideHandler;



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


}
