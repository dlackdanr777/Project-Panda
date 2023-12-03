using UnityEngine;
using Muks.DataBind;

public class UILibrary : UIView
{
    [Tooltip("���̺귯���� �ִ� ��")]
    [SerializeField] private Library _library;
    public Library Library => _library;


    [Tooltip("�ٹ� �޴� ��ũ��Ʈ �ִ� ��")]
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
