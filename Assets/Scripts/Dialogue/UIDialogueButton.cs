using System;
using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;

public class UIDialogueButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Text _buttonText; 

    public Button Button => _button;

    public void Init()
    {
        _button.image.color = new Color(_button.image.color.r, _button.image.color.g, _button.image.color.b, 0);
        _buttonText.color = new Color(_buttonText.color.r, _buttonText.color.g, _buttonText.color.b, 0);
        gameObject.SetActive(false);
    }

    public void ShowButton(string text, Action onComplate = null)
    {
        gameObject.SetActive(true);
        _buttonText.text = text;
        Tween.IamgeAlpha(_button.gameObject, 1, 0.5f, TweenMode.Quadratic);
        Tween.TextAlpha(_buttonText.gameObject, 1, 0.5f, TweenMode.Quadratic, onComplate);
    }

    public void HideButton(Action onComplate = null)
    {

        Tween.IamgeAlpha(_button.gameObject, 0, 0.5f, TweenMode.Quadratic);
        Tween.TextAlpha(_buttonText.gameObject, 0, 0.5f, TweenMode.Quadratic, () => 
        {
            onComplate?.Invoke();
            gameObject.SetActive(false);
        });
    }
}
