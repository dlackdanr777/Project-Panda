using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPhotoAlbum : MonoBehaviour
{
    [SerializeField] private CameraApplication _cameraApplication;

    [SerializeField] private  Image[] _images;

    private int _index;
    private void Awake()
    {
        _images = transform.GetComponentsInChildren<Image>();
    }

    public Image GetImage()
    {
        if (_index >= _images.Length || _index < 0)
            return default;

        return _images[_index];
    }

}
