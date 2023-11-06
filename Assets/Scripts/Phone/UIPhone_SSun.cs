using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UINavigation))]
public class UIPhone_SSun : MonoBehaviour
{
    private UINavigation _uiNav;

    [Tooltip("�ܰ� ��ư")]
    [SerializeField] private Button _borderButton;

    [Tooltip("�κ��丮 ��ư")]
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

    //�κ��丮 �߰�
    private void OnInventoryButtonClicked()
    {
        _uiNav.Push("Inventory");
    }

}
