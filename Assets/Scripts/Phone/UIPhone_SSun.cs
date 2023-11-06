using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UINavigation))]
public class UIPhone_SSun : MonoBehaviour
{
    private UINavigation _uiNav;

    [Tooltip("외곽 버튼")]
    [SerializeField] private Button _borderButton;

    [Tooltip("인벤토리 버튼")]
    [SerializeField] private Button _inventoryButton;
    private void Awake()
    {
        _uiNav = GetComponent<UINavigation>();
        _borderButton.onClick.AddListener(OnBorderButtonClicked);
        _inventoryButton.onClick.AddListener(OnInventoryButtonClicked);
    }
    
    private void OnPhoneButtonClicked()
    { 
        _uiNav.Clear();
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void OnBorderButtonClicked()
    {
        if (_uiNav.Count > 0)
        {

            _uiNav.Pop();
        }
        else
        {
            OnPhoneButtonClicked();
        }
        
    }

    //인벤토리 추가
    private void OnInventoryButtonClicked()
    {
        _uiNav.Push("Inventory");
    }

}
