using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;
using System;
using Random = UnityEngine.Random;

public enum LoadingType
{
    /// <summary>처음 로딩</summary>
    FirstLoading,

    /// <summary>씬 이동</summary>
    SceneChange,
}


public class ChangeSceneManager : SingletonHandler<ChangeSceneManager>
{
    [SerializeField] private Image _fadeImage;

    [SerializeField] private float _fadeDuration;

    [SerializeField] private TweenMode _fadeTweenMode;

    [SerializeField] private FirstLoadingImage[] _firstLoadingImages;

    private bool _isLoading;

    public void Start()
    {
        _isLoading = false;
        Image fadeImage = _fadeImage.GetComponent<Image>();
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
        _fadeImage.gameObject.SetActive(false);

        for(int i = 0, count = _firstLoadingImages.Length; i < count; i++)
        {
            _firstLoadingImages[i].Init(null);
        }
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


    public void FirstLoading()
    {
        int randInt = Random.Range(0, _firstLoadingImages.Length);

        Tween.TransformScale(gameObject, new Vector3(1, 1, 1), 0.1f, TweenMode.Constant, () =>
        {
            ResetFadeImage();
            _firstLoadingImages[randInt].Loading(_fadeDuration);
        });
    }
}
