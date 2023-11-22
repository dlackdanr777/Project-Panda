using System;
using UnityEngine;
using Muks.DataBind;

public class UICameraApp : UIView
{
    [Tooltip("카메라 어플을 놓는 곳")]
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

    private void Awake()
    {
        DataBind.SetButtonValue("ShootingButton", () => StartCoroutine(_cameraApp.ScreenshotByAreaImage(3)));
    }

}
