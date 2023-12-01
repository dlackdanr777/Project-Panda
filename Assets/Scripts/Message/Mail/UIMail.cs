using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMail : UIView
{


    public override void Show()
    {
        gameObject.SetActive(true);
        VisibleState = VisibleState.Appeared;
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
        VisibleState = VisibleState.Disappeared;
    }
}
