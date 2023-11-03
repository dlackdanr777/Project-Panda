using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Album : MonoBehaviour
{
    [Tooltip("ī�޶� �� ��ũ��Ʈ�� �ִ� ��")]
    [SerializeField] private CameraApplication _cameraApp;

    [Tooltip("UIAlbum ��ũ��Ʈ �ִ� ��")]
    [SerializeField] private UIAlbum _uiAlbum;

    private void Awake()
    {
        _cameraApp.OnScreenshotHandler += _uiAlbum.AlbumMenu.CreateSlot;
    }

}
