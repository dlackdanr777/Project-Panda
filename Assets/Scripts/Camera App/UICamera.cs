using System;
using UnityEngine;
using Muks.DataBind;
using Muks.Tween;
using Unity.VisualScripting;

public class UICameraApp : UIView
{
    [Tooltip("ī�޶� ������ ���� ��")]
    [SerializeField] private CameraApplication _cameraApp;

    public event Action OnShowHandler;

    public event Action OnHideHandler;


    public override void Show()
    {
        gameObject.SetActive(true);
        OnShowHandler?.Invoke();
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
        OnHideHandler?.Invoke();
    }



    private void OnEnable()
    {
        DataBind.SetButtonValue("ShootingButton", () => {
            _cameraApp.Screenshot();
            _uiNav.Pop("Camera");
        });

        DataBind.SetButtonValue("HideCameraButton", () =>_uiNav.Pop("Camera"));
    }
}
