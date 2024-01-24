using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;
using System;


public class ChangeSceneManager : SingletonHandler<ChangeSceneManager>
{
    [SerializeField] private Image _fadeImage;

    [SerializeField] private float _fadeDuration;

    [SerializeField] private TweenMode _fadeTweenMode;

    private bool _isLoading;

    public void Start()
    {
        _isLoading = false;
        Image fadeImage = _fadeImage.GetComponent<Image>();
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
        _fadeImage.gameObject.SetActive(false);
    }


     public void ChangeScene(Action onComplete = null)
     {
        if (_isLoading)
            return;

        _isLoading = true;

        GameManager.Instance.FriezeCameraMove = true;
        GameManager.Instance.FriezeCameraZoom = true;
        GameManager.Instance.FirezeInteraction = true;

        _fadeImage.gameObject.SetActive(true);
        Tween.IamgeAlpha(_fadeImage.gameObject, 1, _fadeDuration, _fadeTweenMode, onComplete);
     }


    public void ResetFadeImage(Action onComplete = null)
    {
        _fadeImage.gameObject.SetActive(true);
        Tween.IamgeAlpha(_fadeImage.gameObject, 0, _fadeDuration, _fadeTweenMode, () => 
        {
            onComplete?.Invoke();

            GameManager.Instance.FriezeCameraMove = false;
            GameManager.Instance.FriezeCameraZoom = false;
            GameManager.Instance.FirezeInteraction = false;

            _isLoading = false;
            _fadeImage.gameObject.SetActive(false);
        });
    }
}
