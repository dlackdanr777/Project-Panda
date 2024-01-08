using Muks.DataBind;
using Muks.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInsideWood : UIView
{
    [SerializeField] private GameObject _borderButton;

    [SerializeField] private Vector3 _cameraMovePos;

    private float _tempCameraSize;

    public event Action OnShowHandler;

    public event Action OnHideHandler;

    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        DataBind.SetButtonValue("WoodBorderButton", OnBorderButtonClicked);
        DataBind.SetButtonValue("InventoryButton", OnInventoryButtonClicked);
        DataBind.SetButtonValue("DiaryButton", OnDiaryButtonClicked);
    }

    public override void Show()
    {
        VisibleState = VisibleState.Appearing;

        _tempCameraSize = Camera.main.orthographicSize;
        _uiNav.AllHide();
        
        Tween.TransformMove(Camera.main.gameObject, _cameraMovePos, 1f, TweenMode.Smoothstep, () =>
        {
            Tween.CameraSize(Camera.main.gameObject, 12, 1f, TweenMode.Smoothstep, () =>
            {
                VisibleState = VisibleState.Appeared;

                gameObject.SetActive(true);
                OnShowHandler?.Invoke();
            });


        });

    }

    public override void Hide()
    {
        VisibleState = VisibleState.Disappearing;
        gameObject.SetActive(false);

        Tween.CameraSize(Camera.main.gameObject, _tempCameraSize, 1f, TweenMode.Smoothstep, () =>
        {
            VisibleState = VisibleState.Disappeared;

            _uiNav.AllShow();
            OnHideHandler?.Invoke();
        });


    }

    private void OnDiaryButtonClicked()
    {
        if (!_uiNav.Check("Diary"))
        {
            _uiNav.Push("Diary");
        }
        else
        {
            _uiNav.Pop("Diary");
        }
    }


    private void OnInventoryButtonClicked()
    {
        if (!_uiNav.Check("WishTreeInventory"))
        {
            _uiNav.Push("WishTreeInventory");
        }
        else
        {
            _uiNav.Pop("WishTreeInventory");
        }       
    }


    private void OnBorderButtonClicked()
    {
        _uiNav.Pop();
    }


}
