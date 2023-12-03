using UnityEngine;
using Muks.DataBind;

public class UILibrary : UIView
{
    [Tooltip("라이브러리를 넣는 곳")]
    [SerializeField] private Library _library;
    public Library Library => _library;


    [Tooltip("앨범 메뉴 스크립트 넣는 곳")]
    [SerializeField] private UIAlbum _uiAlbum;
    public UIAlbum UIAlbum => _uiAlbum;


    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        DataBind.SetButtonValue("ExitAlbumButton", OnAlbumButtonClicked);
        
        _uiAlbum.Init();
        
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void Show()
    {
        gameObject.SetActive(true);
    }


    private void OnAlbumButtonClicked()
    {
        _uiNav.Pop("Library");
    }

}
