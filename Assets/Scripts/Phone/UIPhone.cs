using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UINavigation))]
public class UIPhone : MonoBehaviour
{
    private UINavigation _uiNav;

    [Tooltip("�� ��ư")]
    [SerializeField] private Button _phoneButton;

    [Tooltip("�ܰ� ��ư")]
    [SerializeField] private Button _borderButton;

    [Tooltip("ī�޶� ��ư")]
    [SerializeField] private Button _cameraButton;

    [Tooltip("�ٹ� ��ư")]
    [SerializeField] private Button _libraryButton;
    private void Awake()
    {
        _uiNav = GetComponent<UINavigation>();
        _phoneButton.onClick.AddListener(OnPhoneButtonClicked);
        _cameraButton.onClick.AddListener(OnCameraButtonClicked);
        _libraryButton.onClick.AddListener(OnLibaryButtonClicked);
        _borderButton.onClick.AddListener(OnBorderButtonClicked);

    }

    private void OnPhoneButtonClicked()
    { 
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void OnCameraButtonClicked()
    {
        _uiNav.Push("Cameara");
    }

    private void OnLibaryButtonClicked()
    {
        _uiNav.Push("Album");
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
}
