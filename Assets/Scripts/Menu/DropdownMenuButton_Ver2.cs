using UnityEngine;
using UnityEngine.UI;
using System;
using Muks.Tween;

/// <summary>메뉴들이 펼쳐지는 애니메이션 버전의 드롭다운 메뉴 버튼</summary>
public class DropdownMenuButton_Ver2 : DropdownButton
{
    public override void Init()
    {
        _button.gameObject.SetActive(false);
        _dontTouchArea.SetActive(true);
        _defaultImage.gameObject.SetActive(true);
    }


    public override void ShowAnime(float duration, TweenMode tweenMode, Action onCompleted = null)
    {
        Tween.Stop(_defaultImage.gameObject);
        Tween.Stop(_button.gameObject);

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

        _defaultImage.gameObject.SetActive(true);
        _button.gameObject.SetActive(true);
        _dontTouchArea.SetActive(true);

        _defaultImage.color = new Color(_defaultImage.color.r, _defaultImage.color.g, _defaultImage.color.b, 0);
        _button.color = new Color(_button.color.r, _button.color.g, _button.color.b, 1);

        Tween.IamgeAlpha(_defaultImage.gameObject, 1, duration, tweenMode);
        Tween.IamgeAlpha(_button.gameObject, 0, duration, tweenMode, () =>
        {
            _button.gameObject.SetActive(false);
            onCompleted?.Invoke();
        });
    }
}
