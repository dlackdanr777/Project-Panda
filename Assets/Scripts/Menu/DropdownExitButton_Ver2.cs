using Muks.Tween;
using UnityEngine;
using System;


/// <summary>늘어나는 애니메이션 버전의 드롭다운 메뉴 나가기 버튼</summary>
public class DropdownExitButton_Ver2 : DropdownButton
{
    [SerializeField] private GameObject _lopeImage;

    public GameObject DontTouchArea => _dontTouchArea;

    public override void Init()
    {
        _button.gameObject.SetActive(false);
        _lopeImage.gameObject.SetActive(false);
        _dontTouchArea.SetActive(true);
        _defaultImage.gameObject.SetActive(true);
    }


    public override void ShowAnime(float duration, TweenMode tweenMode, Action onCompleted = null)
    {
        Tween.Stop(_defaultImage.gameObject);
        Tween.Stop(_button.gameObject);

        _lopeImage.gameObject.SetActive(true);
        _defaultImage.gameObject.SetActive(true);
        _button.gameObject.SetActive(true);
        _dontTouchArea.SetActive(true);

        _defaultImage.color = new Color(_defaultImage.color.r, _defaultImage.color.g, _defaultImage.color.b, 1);
        _button.color = new Color(_button.color.r, _button.color.g, _button.color.b, 0);
        Tween.IamgeAlpha(_defaultImage.gameObject, 0, duration, tweenMode, () => _defaultImage.gameObject.SetActive(false));
        Tween.IamgeAlpha(_button.gameObject, 1, duration, tweenMode, () =>
        {
            _dontTouchArea.SetActive(false);
            onCompleted?.Invoke();
        });
    }


    public override void HideAnime(float duration, TweenMode tweenMode, Action onCompleted = null)
    {
        Tween.Stop(_defaultImage.gameObject);
        Tween.Stop(_button.gameObject);

        _lopeImage.gameObject.SetActive(true);
        _defaultImage.gameObject.SetActive(true);
        _button.gameObject.SetActive(true);
        _dontTouchArea.SetActive(true);

        _defaultImage.color = new Color(_defaultImage.color.r, _defaultImage.color.g, _defaultImage.color.b, 0);
        _button.color = new Color(_button.color.r, _button.color.g, _button.color.b, 1);

        Tween.IamgeAlpha(_defaultImage.gameObject, 1, duration, tweenMode);
        Tween.IamgeAlpha(_button.gameObject, 0, duration, tweenMode, () =>
        {
            _lopeImage.gameObject.SetActive(false);
            _button.gameObject.SetActive(false);
            onCompleted?.Invoke();
        });
    }
}

