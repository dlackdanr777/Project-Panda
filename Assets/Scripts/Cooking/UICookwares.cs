using Muks.Tween;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICookwares : MonoBehaviour
{
    [SerializeField] private Transform _parent;

    [SerializeField] private GameObject _dontTouchArea;

    private Image[] _cookwareImages;

    private RectTransform _parentRect;

    private float _imageWidth;

    private float _tempAnchoredPosition_X;

    public void Init()
    {
        _parentRect = _parent.GetComponent<RectTransform>();
        _tempAnchoredPosition_X = _parentRect.anchoredPosition.x;
        _cookwareImages = _parent.GetComponentsInChildren<Image>();
        _imageWidth = _cookwareImages[0].rectTransform.rect.height;

        _dontTouchArea.gameObject.SetActive(false);
    }

    public void ChangeImage(int value, Action onComplated = null)
    {
        _dontTouchArea.gameObject.SetActive(true);
        float movePos = _imageWidth * -value;
        Vector2 targetPos = new Vector2(movePos + _tempAnchoredPosition_X, 0);
        Tween.RectTransfromAnchoredPosition(_parent.gameObject, targetPos, 0.5f, TweenMode.Constant, () =>
        {
            _dontTouchArea.gameObject.SetActive(false);
            _parentRect.anchoredPosition = targetPos;
            _tempAnchoredPosition_X += movePos;

            onComplated?.Invoke();
        });
    }
}
