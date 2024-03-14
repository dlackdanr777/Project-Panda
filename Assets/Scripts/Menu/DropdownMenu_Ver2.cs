using Muks.DataBind;
using Muks.Tween;
using UnityEngine;
using UnityEngine.UI;


/// <summary>�޴����� �������� �ִϸ��̼� ������ ��Ӵٿ� �޴�</summary>
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
