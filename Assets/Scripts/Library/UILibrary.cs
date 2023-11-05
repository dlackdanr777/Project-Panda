using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UILibrary : UIView
{
    [Tooltip("앨범 메뉴 스크립트 넣는 곳")]
    public UIAlbum UiAlbum;

    [Tooltip("도감 메뉴 스크립트 넣는 곳")]
    public UIIllustratedGuide UiIllustratedGuide;

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public void Awake()
    {
        Init();
    }

    private void Init()
    {
        DataBinding.SetButtonValue("AlbumButton", OnAlbumButtonClicked);
        DataBinding.SetButtonValue("IllustratedGuideButton", OnIllustratedGuideButtonClicked);
    }

    private void OnEnable()
    {
        OnAlbumButtonClicked();
    }

    private void OnAlbumButtonClicked()
    {
        UiAlbum.gameObject.SetActive(true);
        UiIllustratedGuide.gameObject.SetActive(false);
    }

    private void OnIllustratedGuideButtonClicked()
    {
        UiAlbum.gameObject.SetActive(false);
        UiIllustratedGuide.gameObject.SetActive(true);
    }
}
