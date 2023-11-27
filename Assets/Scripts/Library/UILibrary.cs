using UnityEngine;
using Muks.DataBind;

public class UILibrary : UIView
{
    [Tooltip("�ٹ� �޴� ��ũ��Ʈ �ִ� ��")]
    public UIAlbum UiAlbum;

    /*[Tooltip("���� �޴� ��ũ��Ʈ �ִ� ��")]
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
    }

    private void OnAlbumButtonClicked()
    {
        _uiNav.Pop("Library");
    }

}
