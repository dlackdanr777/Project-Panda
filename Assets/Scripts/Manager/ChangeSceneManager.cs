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

    private Vector3 _tempPos;
    private Vector3 _tempScale;

    private Vector3 _targetPos;
    private Vector3 _targetScale;

    public void Start()
    {
        _tempPos = _fadeImage.rectTransform.anchoredPosition;
        _tempScale = _fadeImage.transform.localScale;

        _targetPos = _tempPos * _fadeScale;
        _targetScale = new Vector3(_fadeScale, _fadeScale, _fadeScale);

        Debug.Log("½ÃÀÛ");
        _fadeImage.gameObject.SetActive(false);
    }


     public void ChangeScene(Action onComplete = null)
     {
         _fadeImage.gameObject.SetActive(true);

        _fadeImage.rectTransform.anchoredPosition = _targetPos;
        _fadeImage.transform.localScale = _targetScale;

        Tween.TransformScale(_fadeImage.gameObject, _tempScale, _fadeDuration, TweenMode.Quadratic);
        Tween.RectTransfromAnchoredPosition(_fadeImage.gameObject, _tempPos, _fadeDuration, TweenMode.Quadratic, onComplete);
     }


    public void HideFadeImage()
    {
        _fadeImage.gameObject.SetActive(false);
    }


    public void ResetFadeImage(Action onComplete = null)
    {
        _fadeImage.gameObject.SetActive(true);

        _fadeImage.rectTransform.anchoredPosition = _tempPos;
        _fadeImage.transform.localScale = _tempScale;

        Tween.TransformScale(_fadeImage.gameObject, _targetScale, _fadeDuration, TweenMode.Quadratic);
        Tween.RectTransfromAnchoredPosition(_fadeImage.gameObject, _targetPos, _fadeDuration, TweenMode.Quadratic, () => 
        {
            onComplete?.Invoke();
            _fadeImage.gameObject.SetActive(false);
        });
    }
}
