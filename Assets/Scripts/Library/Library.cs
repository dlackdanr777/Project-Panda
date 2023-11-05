using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Library : MonoBehaviour
{
    [Tooltip("ī�޶� �� ��ũ��Ʈ�� �ִ� ��")]
    [SerializeField] private CameraApplication _cameraApp;

    [Tooltip("UIAlbum ��ũ��Ʈ �ִ� ��")]
    [SerializeField] private UILibrary _uiLibrary;

    private void Awake()
    {
        _cameraApp.OnScreenshotHandler += _uiLibrary.UiAlbum.CreateSlot;
    }

}
