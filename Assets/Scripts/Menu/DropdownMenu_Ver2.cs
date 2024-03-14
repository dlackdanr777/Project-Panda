using Muks.DataBind;
using Muks.Tween;
using UnityEngine;
using UnityEngine.UI;


/// <summary>메뉴들이 펼쳐지는 애니메이션 버전의 드롭다운 메뉴</summary>
public class DropdownMenu_Ver2 : UIView
{
    [SerializeField] private DropdownMenuButtonGroup_Ver2 _buttonGroup;


    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        _buttonGroup.Init();
    }


    public override void Show()
    {
        VisibleState = VisibleState.Appeared;
        _buttonGroup.ShowAnime();
    }


    public override void Hide()
    {
        VisibleState = VisibleState.Disappeared;
        _buttonGroup.HideAnime();
    }

}
