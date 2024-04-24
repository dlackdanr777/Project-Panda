using UnityEngine;
using UnityEngine.UI;
using System;
using Muks.Tween;
using UnityEngine.Video;

[RequireComponent(typeof(RectTransform))]
public abstract class DropdownButton : MonoBehaviour
{
    [SerializeField] protected Image _button;

    [SerializeField] protected Image _defaultImage;

    [SerializeField] protected GameObject _dontTouchArea;

    protected RectTransform _rectTransform;
    public RectTransform RectTransform => _rectTransform;

    /// <summary>UI의 크기</summary>
    public Vector2 SizeDelta => _rectTransform.sizeDelta;

    /// <summary>UI의 포지션</summary>
    public Vector2 AnchoredPosition => _rectTransform.anchoredPosition;

    protected virtual void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        
    }


    public abstract void Init();


    public abstract void ShowAnime(float duration, TweenMode tweenMode, Action onCompleted = null);


    public abstract void HideAnime(float duration, TweenMode tweenMode, Action onCompleted = null);
}
