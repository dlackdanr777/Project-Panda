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

        DataBind.SetButtonValue("ShowWeatherButton", OnShowWeatherButtonClicked);
        DataBind.SetButtonValue("HideWeatherButton", OnHideWeatherButtonClicked);

        DataBind.SetButtonValue("InventoryButton", OnShowAndHideInventoryButtonClicked);
        DataBind.SetButtonValue("HideInventoryButton", OnHideInventoryButtonClicked);

        DataBind.SetButtonValue("ShowWoodButton", OnShowWoodButtonClicked);
        DataBind.SetButtonValue("HideWoodButton", OnHideWoodButtonClicked);

        DataBind.SetButtonValue("ShowPictureButton", OnShowPictureButtonClicked);
        DataBind.SetButtonValue("HidePictureButton", OnHidePictureButtonClicked);

        DataBind.SetButtonValue("ShowMainUIButton", OnShowMainUIButtonClicked);
        DataBind.SetButtonValue("HideMainUIButton", OnHideMainUIButtonClicked);

        DataBind.SetButtonValue("ShowChallengesButton", OnShowChallengesButtonClicked);
        DataBind.SetButtonValue("CloseChallengesButton", OnHideChallengesButtonClicked);

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


    private void OnInventoryButtonClicked()
    {
        _uiNav.Push("Inventory");

        GameManager.Instance.FirezeInteraction = false;
        GameManager.Instance.FriezeCameraMove = false;
        GameManager.Instance.FriezeCameraZoom = false;
    }

    private void OnShowDiaryButtonClicked()
    {
        _uiNav.Push("Diary");
    }

    private void OnHideDiaryButtonClicked()
    {
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

    private void OnShowWeatherButtonClicked()
    {
        _uiNav.Push("Weather");
    }

    private void OnHideWeatherButtonClicked()
    {
        _uiNav.Pop("Weather");
    }

    private void OnShowAndHideInventoryButtonClicked()
    {
        UIView UIInventory = _uiNav.GetUIView("Inventory");

        //만약 인벤토리 UI가 닫혀있을 경우 연다
        if (UIInventory.VisibleState == VisibleState.Disappeared)
        {
            _uiNav.Push("Inventory");
            GameManager.Instance.FriezeCameraMove = false;
            GameManager.Instance.FirezeInteraction = false;
        }

        //열려 있으면 닫는다.
        else if (UIInventory.VisibleState == VisibleState.Appeared)
        {
            _uiNav.Pop("Inventory");
        }
    }

    private void OnHideInventoryButtonClicked()
    {
        _uiNav.Pop("Inventory");
    }


    public void OnShowWoodButtonClicked()
    {
        _uiNav.Push("InsideWood");
    }


    public void OnHideWoodButtonClicked()
    {
        _uiNav.Pop("InsideWood");
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
        _uiNav.Push("Challenges");
    }

    public void OnHideChallengesButtonClicked()
    {
        _uiNav.Pop("Challenges");
    }
}
