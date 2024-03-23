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
        DataBind.SetButtonValue("Camera Button", OnCameraButtonClicked);
        DataBind.SetButtonValue("ShowMailButton", OnShowMailButtonClicked);
        DataBind.SetButtonValue("HideMailButton", OnHideMailButtonClicked);
        DataBind.SetButtonValue("InventoryButton", OnInventoryButtonClicked);

        DataBind.SetButtonValue("ShowDiaryButton", OnShowDiaryButtonClicked);
        DataBind.SetButtonValue("HideDiaryButton", OnHideDiaryButtonClicked);

        DataBind.SetButtonValue("ShowPhotoButton", OnShowPhotoButtonClicked);
        DataBind.SetButtonValue("HidePhotoButton", OnHidePhotoButtonClicked);

        DataBind.SetButtonValue("ShowAttendanceButton", OnShowAttendanceButtonClicked);
        DataBind.SetButtonValue("HideAttendanceButton", OnHideAttendanceButtonClicked);

        DataBind.SetButtonValue("InventoryButton", OnShowAndHideInventoryButtonClicked);
        DataBind.SetButtonValue("HideInventoryButton", OnHideInventoryButtonClicked);

        DataBind.SetButtonValue("ShowPictureButton", OnShowPictureButtonClicked);
        DataBind.SetButtonValue("HidePictureButton", OnHidePictureButtonClicked);

        DataBind.SetButtonValue("ShowMainUIButton", OnShowMainUIButtonClicked);
        DataBind.SetButtonValue("HideMainUIButton", OnHideMainUIButtonClicked);

        DataBind.SetButtonValue("ShowChallengesButton", OnShowChallengesButtonClicked);
        DataBind.SetButtonValue("CloseChallengesButton", OnHideChallengesButtonClicked);

        DataBind.SetButtonValue("ShowPreferencesButton", OnShowPreferencesButtonClicked);
        DataBind.SetButtonValue("ClosePreferencesButton", OnHidePreferencesButtonClicked);

        DataBind.SetButtonValue("ShowDropDownMenuButton", OnShowDropDownMenuButtonClicked);
        DataBind.SetButtonValue("HideDropDownMenuButton", OnHideDropDownMenuButtonClicked);

        GameManager.Instance.Player.AddItemById("IFI01");
        GameManager.Instance.Player.AddItemById("IFI44");
        GameManager.Instance.Player.AddItemById("CookFd53");
    }

    private void OnCameraButtonClicked()
    {
        _uiNav.Push("Camera");
    }

    private void OnShowDialogue()
    {
        _uiNav.Push("Dialogue");
    }

    private void OnHideDialogue()
    {
        _uiNav.Pop("Dialogue");
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

    private void OnShowPhotoButtonClicked()
    {
        _uiNav.Push("Photo");
    }

    private void OnHidePhotoButtonClicked()
    {
        _uiNav.Pop("Photo");
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
        GameManager.Instance.FriezeCameraMove = false;
        GameManager.Instance.FirezeInteraction = false;
    }

    public void OnHideDropDownMenuButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
        _uiNav.Pop("DropdownMenuButton");
    }

}
