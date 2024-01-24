using Muks.DataBind;
using Muks.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInsideWood : UIView
{
    [SerializeField] private GameObject _borderButton;

    [SerializeField] private Vector3 _cameraMovePos;

    [SerializeField] private SpriteRenderer _inSideWood;

    [SerializeField] private SpriteRenderer _outSideWood;

    private float _tempCameraSize;

    private Vector3 _tmpPos;

    public event Action OnShowHandler;

    public event Action OnHideHandler;

    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);

       _inSideWood.gameObject.SetActive(false);
        DataBind.SetButtonValue("WoodBorderButton", OnBorderButtonClicked);
        DataBind.SetButtonValue("InventoryButton", OnInventoryButtonClicked);
        DataBind.SetButtonValue("DiaryButton", OnDiaryButtonClicked);
    }

    public override void Show()
    {
        VisibleState = VisibleState.Appearing;
        _tempCameraSize = Camera.main.orthographicSize;
        _tmpPos = Camera.main.transform.position;
        _uiNav.HideMainUI();

        _inSideWood.gameObject.SetActive(true);
        _inSideWood.color = new Color(_inSideWood.color.r, _inSideWood.color.g, _inSideWood.color.b, 0);

        Tween.TransformMove(Camera.main.gameObject, _cameraMovePos, 1f, TweenMode.Smoothstep, () =>
        {
            Tween.SpriteRendererAlpha(_inSideWood.gameObject, 1, 1, TweenMode.Smoothstep);
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

        _inSideWood.color = new Color(_inSideWood.color.r, _inSideWood.color.g, _inSideWood.color.b, 1);
        Vector3 targetPos = _tmpPos;
        Tween.SpriteRendererAlpha(_inSideWood.gameObject, 0, 1, TweenMode.Smoothstep);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 1, TweenMode.Smoothstep);
        Tween.CameraSize(Camera.main.gameObject, _tempCameraSize, 1f, TweenMode.Smoothstep, () =>
        {
            VisibleState = VisibleState.Disappeared;
            _inSideWood.gameObject.SetActive(false);
            _uiNav.ShowMainUI();
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
