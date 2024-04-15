using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UINavigation))]
public class UIMainScene : MonoBehaviour
{

    private UINavigation _uiNav;

    private void Awake()
    {
        _uiNav = GetComponent<UINavigation>();
        DataBind.SetUnityActionValue("Camera Button", OnCameraButtonClicked);
        DataBind.SetUnityActionValue("ShowMailButton", OnShowMailButtonClicked);
        DataBind.SetUnityActionValue("HideMailButton", OnHideMailButtonClicked);
        DataBind.SetUnityActionValue("InventoryButton", OnInventoryButtonClicked);

        DataBind.SetUnityActionValue("ShowDiaryButton", OnShowDiaryButtonClicked);
        DataBind.SetUnityActionValue("HideDiaryButton", OnHideDiaryButtonClicked);

        DataBind.SetUnityActionValue("ShowAttendanceButton", OnShowAttendanceButtonClicked);
        DataBind.SetUnityActionValue("HideAttendanceButton", OnHideAttendanceButtonClicked);

        DataBind.SetUnityActionValue("InventoryButton", OnShowAndHideInventoryButtonClicked);
        DataBind.SetUnityActionValue("HideInventoryButton", OnHideInventoryButtonClicked);

        DataBind.SetUnityActionValue("ShowPictureButton", OnShowPictureButtonClicked);
        DataBind.SetUnityActionValue("HidePictureButton", OnHidePictureButtonClicked);

        DataBind.SetUnityActionValue("ShowMainUIButton", OnShowMainUIButtonClicked);
        DataBind.SetUnityActionValue("HideMainUIButton", OnHideMainUIButtonClicked);

        DataBind.SetUnityActionValue("ShowChallengesButton", OnShowChallengesButtonClicked);
        DataBind.SetUnityActionValue("CloseChallengesButton", OnHideChallengesButtonClicked);

        DataBind.SetUnityActionValue("ShowPreferencesButton", OnShowPreferencesButtonClicked);
        DataBind.SetUnityActionValue("ClosePreferencesButton", OnHidePreferencesButtonClicked);

        DataBind.SetUnityActionValue("ShowDropDownMenuButton", OnShowDropDownMenuButtonClicked);
        DataBind.SetUnityActionValue("HideDropDownMenuButton", OnHideDropDownMenuButtonClicked);

        DataBind.SetUnityActionValue("ShowNoticeButton", OnShowNoticeButtonClicked);
        DataBind.SetUnityActionValue("HideNoticeButton", OnHideNoticeButtonClicked);
    }


    private void OnCameraButtonClicked()
    {
        _uiNav.Push("Camera");
    }

    private void OnShowMailButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
        _uiNav.Push("Mail");
    }

    private void OnHideMailButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
        _uiNav.Pop("Mail");
    }


    private void OnInventoryButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
        _uiNav.Push("Inventory");
    }

    private void OnShowDiaryButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
        _uiNav.Push("Diary");
    }

    private void OnHideDiaryButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
        _uiNav.Pop("Diary");
    }

    private void OnShowAttendanceButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
        _uiNav.Push("Attendance");
    }

    private void OnHideAttendanceButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
        _uiNav.Pop("Attendance");
    }

    private void OnShowAndHideInventoryButtonClicked()
    {
        UIView UIInventory = _uiNav.GetUIView("Inventory");

        //만약 인벤토리 UI가 닫혀있을 경우 연다
        if (UIInventory.VisibleState == VisibleState.Disappeared)
        {
            SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
            _uiNav.Push("Inventory");
/*            GameManager.Instance.FriezeCameraMove = false;
            GameManager.Instance.FirezeInteraction = false;*/
        }

        //열려 있으면 닫는다.
        else if (UIInventory.VisibleState == VisibleState.Appeared)
        {
            SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
            _uiNav.Pop("Inventory");
        }
    }

    private void OnHideInventoryButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
        _uiNav.Pop("Inventory");
    }


    public void OnShowPictureButtonClicked()
    {
        _uiNav.Push("Picture");
    }


    public void OnHidePictureButtonClicked()
    {
        _uiNav.Pop("Picture");
    }



    public void OnHideMainUIButtonClicked()
    {
        _uiNav.HideMainUI();
    }

    public void OnShowMainUIButtonClicked()
    {
        _uiNav.ShowMainUI();
    }

    public void OnShowChallengesButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
        _uiNav.Push("Challenges");
    }

    public void OnHideChallengesButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
        _uiNav.Pop("Challenges");
    }

    public void OnShowPreferencesButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
        _uiNav.Push("Preferences");
    }

    public void OnHidePreferencesButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
        _uiNav.Pop("Preferences");
    }

    public void OnShowDropDownMenuButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
        _uiNav.Push("DropdownMenuButton");
    }

    public void OnHideDropDownMenuButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
        _uiNav.Pop("DropdownMenuButton");
    }

    public void OnShowNoticeButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
        _uiNav.Push("UINotice");
    }

    public void OnHideNoticeButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
        _uiNav.Pop("UINotice");
    }

}
