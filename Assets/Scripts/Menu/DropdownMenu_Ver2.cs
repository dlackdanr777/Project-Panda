using Muks.DataBind;
using Muks.Tween;
using UnityEngine;
using UnityEngine.UI;


/// <summary>메뉴들이 펼쳐지는 애니메이션 버전의 드롭다운 메뉴</summary>
public class DropdownMenu_Ver2 : MonoBehaviour
{
    [SerializeField] private DropdownMenuButtonGroup_Ver2 _buttonGroup;

    private void Awake()
    {
        DataBind.SetButtonValue("ShowDropdownMenuButton_Ver2", Show);
        DataBind.SetButtonValue("HideDropdownMenuButton_Ver2", Hide);
    }


    private void Start()
    {
        _buttonGroup.Init();
    }


    private void Show()
    {
        _buttonGroup.ShowAnime();
    }


    private void Hide()
    {
        _buttonGroup.HideAnime();
    }

}
