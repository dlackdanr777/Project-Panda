using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;
using System;

public class ChangeSceneManager : SingletonHandler<ChangeSceneManager>
{
    [SerializeField] private Image _fadeImage;
    [SerializeField] private Image _fadeBackgroundImage;

    [SerializeField] private float _fadeScale;
    [SerializeField] private float _fadeDuration;

    private Vector3 _tempPos;
    private Vector3 _tempScale;


    public void Start()
    {
        _tempPos = _fadeImage.rectTransform.anchoredPosition;
        _tempScale = _fadeImage.transform.localScale;
        _fadeImage.gameObject.SetActive(false);
    }

   /* public void ResetFadeImage( Action onComplete = null)
    {
        _fadeImage.gameObject.SetActive(true);
        Tween.TransformScale(_fadeImage.gameObject, new Vector3(_fadeScale, _fadeScale, _fadeScale), _fadeDuration, TweenMode.Quadratic);
        Tween.TransformScale(_fadeBackgroundImage.gameObject, new Vector3( 1/_fadeScale, 1/_fadeScale, 1/_fadeScale), _fadeDuration, TweenMode.Quadratic);

        Vector3 targetPos = _fadeImage.rectTransform.anchoredPosition * _fadeScale;
        Tween.RectTransfromAnchoredPosition(_fadeImage.gameObject, targetPos, _fadeDuration, TweenMode.Quadratic, onComplete);
    }*/


   /* public void ChangeScene(Action onComplete = null)
    {
        _fadeImage.gameObject.SetActive(true);

        Vector3 targetPos = new Vector3(_tempScale.x * _fadeImage.transform.localScale.x, _tempScale.y * _fadeImage.transform.localScale.y, _tempScale.z * _fadeImage.transform.localScale.z);

        Tween.TransformScale(_fadeImage.gameObject, _tempScale, _fadeDuration, TweenMode.Quadratic);
        Tween.TransformScale(_fadeBackgroundImage.gameObject, targetPos, _fadeDuration, TweenMode.Quadratic);

        Tween.RectTransfromAnchoredPosition(_fadeImage.gameObject, _tempPos, _fadeDuration, TweenMode.Quadratic, () =>
       {
           onComplete?.Invoke();
           _fadeImage.gameObject.SetActive(false);
       });
    }*/



     public void ChangeScene(Action onComplete = null)
     {
         _fadeImage.gameObject.SetActive(true);
         Tween.TransformScale(_fadeImage.gameObject, new Vector3(_fadeScale, _fadeScale, _fadeScale), _fadeDuration, TweenMode.Quadratic);

         Vector3 targetPos = _fadeImage.rectTransform.anchoredPosition * _fadeScale;
         Tween.RectTransfromAnchoredPosition(_fadeImage.gameObject, targetPos, _fadeDuration, TweenMode.Quadratic, onComplete);
     }


    public void HideFadeImage()
    {
        _fadeImage.gameObject.SetActive(false);
    }


    public void ResetFadeImage(Action onComplete = null)
    {
        _fadeImage.gameObject.SetActive(true);

        Tween.TransformScale(_fadeImage.gameObject, _tempScale, _fadeDuration, TweenMode.Quadratic);
        Tween.RectTransfromAnchoredPosition(_fadeImage.gameObject, _tempPos, _fadeDuration, TweenMode.Quadratic, () => 
        {
            onComplete?.Invoke();
            _fadeImage.gameObject.SetActive(false);
        });
    }
}
