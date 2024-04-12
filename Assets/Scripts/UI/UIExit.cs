
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

    /// <summary>종료 버튼을 누르면 실행되는 함수</summary>
    private void AppExitOkButtonClicked()
    {
        Application.Quit();
    }


    /// <summary>취소 버튼을 누르면 실행되는 함수</summary>
    private void AppExitCancelButtonClicked()
    {
        _uiNav.Pop();
    }
}
