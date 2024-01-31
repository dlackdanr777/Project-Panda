using System;
using UnityEngine;
using Muks.DataBind;
using Muks.Tween;
using Unity.VisualScripting;
using UnityEngine.UI;

public class UICameraApp : UIView
{
    [Tooltip("카메라 어플을 놓는 곳")]
    [SerializeField] private CameraApplication _cameraApp;

    [Tooltip("이동용 카메라 놓는 곳")]
    [SerializeField] private ScreenshotCamera _screenshotCamera;

    [Tooltip("카메라 앵글 이미지")]
    [SerializeField] private GameObject _angleImage;


    public event Action OnHideHandler;


    public override void Show()
    {
        gameObject.SetActive(true);
        _screenshotCamera.gameObject.SetActive(true);
        _angleImage.SetActive(true);
        Debug.Log("카메라 실행");
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
        _screenshotCamera.gameObject.SetActive(false);
        OnHideHandler?.Invoke();
    }



    private void OnEnable()
    {
        DataBind.SetButtonValue("ShootingButton", () => {
            _angleImage.SetActive(false);
            _cameraApp.Screenshot();
            Invoke("PopCamera", 0.1f);
        });

        DataBind.SetButtonValue("HideCameraButton", () =>_uiNav.Pop("Camera"));
    }

    private void PopCamera()
    {
        _uiNav.Pop("Camera");
    }
}
