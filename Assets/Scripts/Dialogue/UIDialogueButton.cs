using System;
using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;
using TMPro;

public class UIDialogueButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    [SerializeField] private TextMeshProUGUI _buttonText;

    [SerializeField] private Image _outerImage;

    [SerializeField] private Image _innerImage;

    [SerializeField] private float _animeDuration;
    public Button Button => _button;

    public void Init()
    {
        Disabled();
        gameObject.SetActive(false);
    }

    public void ShowButton(string text, Action onComplate = null)
    {
        gameObject.SetActive(true);
        _buttonText.text = text;
        Tween.IamgeAlpha(_button.gameObject, 1, _animeDuration, TweenMode.Quadratic);
        Tween.IamgeAlpha(_outerImage.gameObject, 1, _animeDuration, TweenMode.Quadratic);
        Tween.IamgeAlpha(_innerImage.gameObject, 1, _animeDuration, TweenMode.Quadratic);
        Tween.TMPAlpha(_buttonText.gameObject, 1, _animeDuration, TweenMode.Quadratic, onComplate);
    }

    public void HideButton(Action onComplate = null)
    {

        Tween.IamgeAlpha(_button.gameObject, 0, _animeDuration, TweenMode.Quadratic);
        Tween.IamgeAlpha(_outerImage.gameObject, 0, _animeDuration, TweenMode.Quadratic);
        Tween.IamgeAlpha(_innerImage.gameObject, 0, _animeDuration, TweenMode.Quadratic);
        Tween.TMPAlpha(_buttonText.gameObject, 0, _animeDuration, TweenMode.Quadratic, () => 
        {
            onComplate?.Invoke();
            gameObject.SetActive(false);
        });
    }

    public void Disabled()
    {

        _button.image.color = new Color(_button.image.color.r, _button.image.color.g, _button.image.color.b, 0);
        _outerImage.color = new Color(_outerImage.color.r, _outerImage.color.g, _outerImage.color.b, 0);
        _innerImage.color = new Color(_innerImage.color.r, _innerImage.color.g, _innerImage.color.b, 0);
        _buttonText.color = new Color(_buttonText.color.r, _buttonText.color.g, _buttonText.color.b, 0);
    }
}
