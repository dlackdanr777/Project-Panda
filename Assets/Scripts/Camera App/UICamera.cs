using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICamera : UIView
{
    [Tooltip("ī�޶� ������ ���� ��")]
    [SerializeField] private CameraApplication _cameraApp;

    [SerializeField] private Button _shootingButton;
    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
       // _shootingButton.onClick.AddListener(() => StartCoroutine(_cameraApp.ScreenshotByAreaImage(3)));

        DataBinding.SetButtonValue("ShootingButton", () => StartCoroutine(_cameraApp.ScreenshotByAreaImage(3)));
    }

}
