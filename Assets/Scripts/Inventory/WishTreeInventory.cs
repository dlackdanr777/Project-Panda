using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishTreeInventory : UIView
{
    public override void Hide()
    {
        VisibleState = VisibleState.Disappeared;
        gameObject.SetActive(false);

    }

    public override void Show() 
    {
        VisibleState = VisibleState.Appeared;
        gameObject.SetActive(true);
    }
}
