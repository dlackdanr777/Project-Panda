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
        CameraController.FriezePos = true;
        OnShowHandler?.Invoke();
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
        CameraController.FriezePos = false;
        OnHideHandler?.Invoke();
    }



    private void OnEnable()
    {
        DataBind.SetButtonValue("ShootingButton", () => {
            _cameraApp.Screenshot();
            _uiNav.Pop();
        });

        DataBind.SetButtonValue("HideCameraButton", () =>_uiNav.Pop());
    }
}
