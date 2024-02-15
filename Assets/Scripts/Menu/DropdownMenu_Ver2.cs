using Muks.DataBind;
using Muks.Tween;
using UnityEngine;
using UnityEngine.UI;


/// <summary>�޴����� �������� �ִϸ��̼� ������ ��Ӵٿ� �޴�</summary>
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
