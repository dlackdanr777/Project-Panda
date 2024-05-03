using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;
using System;
using Random = UnityEngine.Random;

public enum LoadingType
{
    /// <summary>ó�� �ε�</summary>
    FirstLoading,

    /// <summary>�� �̵�</summary>
    SceneChange,
}


/// <summary>ȭ�� ��ȯ ȿ���� �ִ� �̱��� �Ŵ���</summary>
public class FadeInOutManager : SingletonHandler<FadeInOutManager>
{
    [SerializeField] private Image _fadeImage;

    [SerializeField] private float _fadeDuration;

    [SerializeField] private float _fadeOutWaitDuration;

    [SerializeField] private TweenMode _fadeTweenMode;

    [SerializeField] private FirstLoadingImage[] _firstLoadingImages;

    private bool _isLoading;


    public event Action OnFadeInHandler;
    public event Action OnEndFadeInHandler;
    public event Action OnFadeOutHandler;
    public event Action OnEndFadeOutHandler;
    public event Action OnEndFirstLoadingHandler;


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


    /// <summary>FadeInȿ���� �ִ� �Լ�(duration�� 0�� ��� �⺻ ��, onComplete�� FadeInȿ���� ����� �� ����)</summary>
    public void FadeIn(float duration = 0, Action onComplete = null)
    {
        if (_isLoading)
            return;

        _isLoading = true;

        GameManager.Instance.FriezeCameraMove = true;
        GameManager.Instance.FriezeCameraZoom = true;
        GameManager.Instance.FirezeInteraction = true;

        _fadeImage.gameObject.SetActive(true);

        duration = duration == 0 ? _fadeDuration : duration;
        Tween.IamgeAlpha(_fadeImage.gameObject, 1, duration, _fadeTweenMode, () =>
        {
            onComplete?.Invoke();
            OnEndFadeInHandler?.Invoke();
        });

        OnFadeInHandler?.Invoke();
    }


    /// <summary>FadeOutȿ���� �ִ� �Լ�(duration�� 0�� ��� �⺻ ��, onComplete�� FadeInȿ���� ����� �� ����)</summary>
    public void FadeOut(float duration = 0, float waitDuration = 0, Action onComplete = null)
    {
        _fadeImage.gameObject.SetActive(true);

        duration = duration == 0 ? _fadeDuration : duration;
        waitDuration = waitDuration == 0 ? _fadeOutWaitDuration : waitDuration;

        Tween.TransformMove(gameObject, transform.position, waitDuration, TweenMode.Constant, () =>
        {
            Tween.IamgeAlpha(_fadeImage.gameObject, 0, duration, _fadeTweenMode, () =>
            {
                onComplete?.Invoke();

                GameManager.Instance.FriezeCameraMove = false;
                GameManager.Instance.FriezeCameraZoom = false;
                GameManager.Instance.FirezeInteraction = false;

                _fadeImage.gameObject.SetActive(false);
                _isLoading = false;
                OnEndFadeOutHandler?.Invoke();
            });
        });

        OnFadeOutHandler?.Invoke();
    }


    /// <summary>ù �ε��� ȭ�� ��ȯ ȿ���� �ִ� �Լ�(duration�� 0�� ��� �⺻ ��)</summary>
    public void FirstFadeInOut(float duration = 0, float waitDuration = 0)
    {
        int randInt = Random.Range(0, _firstLoadingImages.Length);

        duration = duration == 0 ? _fadeDuration : duration;
        waitDuration = waitDuration == 0 ? _fadeOutWaitDuration : waitDuration;
        
        Tween.TransformMove(gameObject, transform.position, waitDuration, TweenMode.Constant, () =>
        {
            FadeOut(duration, waitDuration + 2);
            _firstLoadingImages[randInt].Loading(duration * 0.5f);
        });
    }
}
