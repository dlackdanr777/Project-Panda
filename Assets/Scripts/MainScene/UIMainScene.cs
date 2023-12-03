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
        DataBind.SetButtonValue("DiaryButton", OnDiaryButtonClicked);

        DataBind.SetButtonValue("ShowPhotoButton", OnShowPhotoButtonClicked);
        DataBind.SetButtonValue("HidePhotoButton", OnHidePhotoButtonClicked);
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
        _uiNav.Push("Mail");
    }

    private void OnHideMailButtonClicked()
    {
        _uiNav.Pop("Mail");
    }

    private void OnDiaryButtonClicked()
    {
        _uiNav.Push("Diary");
    }
    private void OnInventoryButtonClicked()
    {
        _uiNav.Push("Inventory");
    }


    private void OnShowPhotoButtonClicked()
    {
        _uiNav.Push("Photo");
    }

    private void OnHidePhotoButtonClicked()
    {
        _uiNav.Pop("Photo");
    }

}
