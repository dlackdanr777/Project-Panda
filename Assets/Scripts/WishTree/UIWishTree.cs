using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWishTree : MonoBehaviour
{
    private UINavigation_origin _uiNav;

    private string _value;

    private void Awake()
    {
        _uiNav = GetComponent<UINavigation_origin>();

        DataBind.SetUnityActionValue("WoodButton", OnInsideWoodButtonClicked);
        DataBind.SetUnityActionValue("WishTreeBorderButton", OnBorderButtonClicked);
    }

    private void OnInsideWoodButtonClicked()
    {
        _uiNav.Push("InsideWood");
    }

    private void OnBorderButtonClicked()
    {
        _uiNav.Pop();

    }
}