using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWood : MonoBehaviour
{
    private UINavigation_origin _uiNav;

    private string _value;

    [SerializeField] private GameObject _borderButton;

    private void Awake()
    {
        _uiNav = GetComponent<UINavigation_origin>();

        DataBind.SetUnityActionValue("InventoryButton", OnInventoryButtonClicked);
        DataBind.SetUnityActionValue("DiaryButton", OnDiaryButtonClicked);
        DataBind.SetUnityActionValue("WoodBorderButton", OnBorderButtonClicked);
    }

    private void OnDiaryButtonClicked()
    {
        if (!_borderButton.activeSelf)
        {
            _borderButton.SetActive(true);
        }
        _uiNav.Push("Diary");
    }
    private void OnInventoryButtonClicked()
    {
        if (!_borderButton.activeSelf)
        {
            _borderButton.SetActive(true);
        }
        _uiNav.Push("Inventory");
    }

    private void OnBorderButtonClicked()
    {
        if (_uiNav.Count == 0)
        {
            _borderButton.SetActive(false);
        }
        _uiNav.Pop();

    }

    private void HidePanel()
    {
    }
}