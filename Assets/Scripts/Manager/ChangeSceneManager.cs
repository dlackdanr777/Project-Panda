using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;
using System;


public class ChangeSceneManager : SingletonHandler<ChangeSceneManager>
{
    [SerializeField] private Image _fadeImage;

    [SerializeField] private float _fadeScale;

    [SerializeField] private float _fadeDuration;

    [SerializeField] private TweenMode _fadeTweenMode;

    private Vector3 _tempPos;
    private Vector3 _tempSize;

    private Vector3 _targetPos;
    private Vector3 _targetSize;

    public void Start()
    {
        _tempPos = _fadeImage.rectTransform.anchoredPosition;
        _tempSize = _fadeImage.rectTransform.sizeDelta;

        _targetPos = _tempPos * _fadeScale;
        _targetSize = _tempSize * _fadeScale;

        _fadeImage.gameObject.SetActive(false);
    }


     public void ChangeScene(Action onComplete = null)
     {
         _fadeImage.gameObject.SetActive(true);

        _fadeImage.rectTransform.anchoredPosition = _targetPos;
        _fadeImage.rectTransform.sizeDelta = _targetSize;

        Tween.RectTransfromSizeDelta(_fadeImage.gameObject, _tempSize, _fadeDuration, _fadeTweenMode);
        Tween.RectTransfromAnchoredPosition(_fadeImage.gameObject, _tempPos, _fadeDuration, _fadeTweenMode, onComplete);
     }


    public void HideFadeImage()
    {
        //_fadeImage.gameObject.SetActive(false);
    }


    public void ResetFadeImage(Action onComplete = null)
    {
        _fadeImage.gameObject.SetActive(true);

        _fadeImage.rectTransform.anchoredPosition = _tempPos;
        _fadeImage.rectTransform.sizeDelta = _tempSize;

        Tween.RectTransfromSizeDelta(_fadeImage.gameObject, _targetSize, _fadeDuration, _fadeTweenMode);
        Tween.RectTransfromAnchoredPosition(_fadeImage.gameObject, _targetPos, _fadeDuration, _fadeTweenMode, () => 
        {
            onComplete?.Invoke();
            _fadeImage.gameObject.SetActive(false);
        });
    }
}
