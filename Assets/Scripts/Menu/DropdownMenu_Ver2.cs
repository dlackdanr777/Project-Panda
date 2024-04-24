using Muks.DataBind;
using Muks.Tween;
using UnityEngine;
using UnityEngine.UI;


/// <summary>메뉴들이 펼쳐지는 애니메이션 버전의 드롭다운 메뉴</summary>
public class DropdownMenu_Ver2 : UIView
{
    [Header("Components")]
    [SerializeField] private DropdownMenuButtonGroup_Ver2 _buttonGroup;
    [SerializeField] private GameObject _backgroundImage;
    [SerializeField] private GameObject _backgroundButton;


    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        _backgroundImage.SetActive(false);
        _backgroundButton.SetActive(false);
        _buttonGroup.Init();
    }


    public override void Show()
    {
        VisibleState = VisibleState.Appeared;
        _backgroundImage.SetActive(true);
        _buttonGroup.ShowAnime(() => _backgroundButton.SetActive(true));
    }


    public override void Hide()
    {
        VisibleState = VisibleState.Disappeared;
        _backgroundButton.SetActive(false);
        _buttonGroup.HideAnime(() => _backgroundImage.SetActive(false));
    }

}
