using UnityEngine;
using Muks.DataBind;

public class UILibrary : UIView
{
    [Tooltip("앨범 메뉴 스크립트 넣는 곳")]
    public UIAlbum UiAlbum;

    /*[Tooltip("도감 메뉴 스크립트 넣는 곳")]
    public UIIllustratedGuide UiIllustratedGuide;*/

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void Show()
    {
        gameObject.SetActive(true);
    }



    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        DataBind.SetButtonValue("ExitAlbumButton", OnAlbumButtonClicked);
        //DataBind.SetButtonValue("IllustratedGuideButton", OnIllustratedGuideButtonClicked);
    }

    private void OnAlbumButtonClicked()
    {
        _uiNav.Pop();
        //UiIllustratedGuide.gameObject.SetActive(false);
    }

}
