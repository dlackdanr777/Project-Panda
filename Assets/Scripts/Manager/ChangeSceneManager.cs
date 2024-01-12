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

    [Space]
    [SerializeField] private GameObject _dontTouchArea;

    private Vector3 _tempPos;
    private Vector3 _tempSize;

    private Vector3 _targetPos;
    private Vector3 _targetSize;

    private bool _isLoading;

    public void Start()
    {
        _tempPos = _fadeImage.rectTransform.anchoredPosition;
        _tempSize = _fadeImage.rectTransform.sizeDelta;

        _targetPos = _tempPos * _fadeScale;
        _targetSize = _tempSize * _fadeScale;

        _isLoading = false;

        _fadeImage.gameObject.SetActive(false);
        _dontTouchArea.SetActive(false);
    }


     public void ChangeScene(Action onComplete = null)
     {
        if (_isLoading)
            return;

        _dontTouchArea.SetActive(true);
        _fadeImage.gameObject.SetActive(true);

        _isLoading = true;

        GameManager.Instance.FriezeCameraMove = true;
        GameManager.Instance.FriezeCameraZoom = true;
        GameManager.Instance.FirezeInteraction = true;

        _fadeImage.rectTransform.anchoredPosition = _targetPos;
        _fadeImage.rectTransform.sizeDelta = _targetSize;

        Tween.RectTransfromSizeDelta(_fadeImage.gameObject, _tempSize, _fadeDuration, _fadeTweenMode);
        Tween.RectTransfromAnchoredPosition(_fadeImage.gameObject, _tempPos, _fadeDuration, _fadeTweenMode, onComplete);
     }


    public void HideFadeImage()
    {
        _dontTouchArea.SetActive(true);
        _fadeImage.gameObject.SetActive(false);
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

            GameManager.Instance.FriezeCameraMove = false;
            GameManager.Instance.FriezeCameraZoom = false;
            GameManager.Instance.FirezeInteraction = false;

            _isLoading = false;

            _fadeImage.gameObject.SetActive(false);
            _dontTouchArea.SetActive(false);
        });
    }
}
