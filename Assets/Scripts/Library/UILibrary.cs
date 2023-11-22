using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.DataBind;

public class UILibrary : UIView
{
    [Tooltip("�ٹ� �޴� ��ũ��Ʈ �ִ� ��")]
    public UIAlbum UiAlbum;

    [Tooltip("���� �޴� ��ũ��Ʈ �ִ� ��")]
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
        DataBind.SetButtonValue("AlbumButton", OnAlbumButtonClicked);
        DataBind.SetButtonValue("IllustratedGuideButton", OnIllustratedGuideButtonClicked);
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
