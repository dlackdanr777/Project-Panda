
using UnityEngine;
using Muks.DataBind;

public class UIExit : UIView
{
    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);

        DataBind.SetUnityActionValue("AppExitOkButton", AppExitOkButtonClicked);
        DataBind.SetUnityActionValue("AppExitCancelButton", AppExitCancelButtonClicked);

        gameObject.SetActive(false);
    }

    public override void Show()
    {
        VisibleState = VisibleState.Appeared;
        gameObject.SetActive(true);
    }


    public override void Hide()
    {
        VisibleState = VisibleState.Disappeared;
        gameObject.SetActive(false);
    }

    /// <summary>���� ��ư�� ������ ����Ǵ� �Լ�</summary>
    private void AppExitOkButtonClicked()
    {
        Application.Quit();
    }


    /// <summary>��� ��ư�� ������ ����Ǵ� �Լ�</summary>
    private void AppExitCancelButtonClicked()
    {
        _uiNav.Pop();
    }
}
