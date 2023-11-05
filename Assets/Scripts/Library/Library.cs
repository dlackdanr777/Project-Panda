using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Library : MonoBehaviour
{
    [Tooltip("카메라 앱 스크립트를 넣는 곳")]
    [SerializeField] private CameraApplication _cameraApp;

    [Tooltip("UIAlbum 스크립트 넣는 곳")]
    [SerializeField] private UILibrary _uiLibrary;

    private void Awake()
    {
        _cameraApp.OnScreenshotHandler += _uiLibrary.UiAlbum.CreateSlot;
    }

}
