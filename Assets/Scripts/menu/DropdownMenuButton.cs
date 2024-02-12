using UnityEngine;
using UnityEngine.UI;
using System;
using Muks.Tween;

public class DropdownMenuButton : MonoBehaviour
{
    [SerializeField] private Image _button;

    [SerializeField] private Image _circleImage;

    [SerializeField] private GameObject _dontTouchArea;


    public void Init()
    {
        _button.gameObject.SetActive(false);
        _dontTouchArea.SetActive(true);
        _circleImage.gameObject.SetActive(true);
    }


    public void ShowAnime(float duration, TweenMode tweenMode, Action onCompleted = null)
    {
        Tween.Stop(_circleImage.gameObject);
        Tween.Stop(_button.gameObject);

        _circleImage.gameObject.SetActive(true);
        _button.gameObject.SetActive(true);
        _dontTouchArea.SetActive(true);

        _circleImage.color = new Color(_circleImage.color.r, _circleImage.color.g, _circleImage.color.b, 1);
        _button.color = new Color(_button.color.r, _button.color.g, _button.color.b, 0);
        Tween.IamgeAlpha(_circleImage.gameObject, 0, duration, tweenMode, () => _circleImage.gameObject.SetActive(false));
        Tween.IamgeAlpha(_button.gameObject, 1, duration, tweenMode, () =>
        {
            _dontTouchArea.SetActive(false);
            onCompleted?.Invoke();
        });
    }

    public void HideAnime(float duration, TweenMode tweenMode, Action onCompleted = null)
    {
        Tween.Stop(_circleImage.gameObject);
        Tween.Stop(_button.gameObject);

        _circleImage.gameObject.SetActive(true);
        _button.gameObject.SetActive(true);
        _dontTouchArea.SetActive(true);

        _circleImage.color = new Color(_circleImage.color.r, _circleImage.color.g, _circleImage.color.b, 0);
        _button.color = new Color(_button.color.r, _button.color.g, _button.color.b, 1);

        Tween.IamgeAlpha(_circleImage.gameObject, 1, duration, tweenMode);
        Tween.IamgeAlpha(_button.gameObject, 0, duration, tweenMode, () =>
        {
            _button.gameObject.SetActive(false);
            onCompleted?.Invoke();
        });

    }
}
