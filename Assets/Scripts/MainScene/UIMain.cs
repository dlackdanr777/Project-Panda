using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.DataBind;

public class UIMain : UIView
{
    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void Show()
    {
        gameObject.SetActive(true);
    }


    public void OnEnable()
    {

        DataBind.SetButtonValue("Show Camera Button", () => _uiNav.Push("Camera"));
        DataBind.SetButtonValue("ShowAlbumButton", () => _uiNav.Push("Library"));
    }
}
