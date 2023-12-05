using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDaySlot : MonoBehaviour
{
    [SerializeField] private Image _image;

    [SerializeField] private Image _complatedImage;

    [SerializeField] private Sprite _nomalSprite;

    [SerializeField] private Sprite _complatedSprite;

    public void Init(bool isComplated)
    {
        _image.sprite = isComplated ? _complatedSprite : _nomalSprite;
        _complatedImage.gameObject.SetActive(isComplated);
    }
}
