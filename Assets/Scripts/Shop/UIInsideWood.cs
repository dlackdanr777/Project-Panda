using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInsideWood : UIView
{
    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void Show()
    {
        gameObject.SetActive(true);
    }
    
}
