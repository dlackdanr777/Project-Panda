using Muks.Tween;
using System.Collections;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStory1OutroScene : UIInOutroScene
{
    [SerializeField] private Image _fadeImage;
    [SerializeField] private TextMeshProUGUI _endText;

    public override void Init()
    {
        _fadeImage.gameObject.SetActive(false);
        _endText.gameObject.SetActive(false);
        base.Init();
    }


    public void StartFadeIn(float duration)
    {
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.color = new Color(0, 0, 0, 0);

        Tween.IamgeAlpha(_fadeImage.gameObject, 1, duration, TweenMode.Constant);
    }


    public void StartFadeOut(float duration)
    {
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.color = new Color(0, 0, 0, 1);

        Tween.IamgeAlpha(_fadeImage.gameObject, 0, duration, TweenMode.Constant, () => _fadeImage.gameObject.SetActive(false)); ;
    }


    public void StartEndText(float duration)
    {
        _endText.gameObject.SetActive(true);
        _endText.color = new Color(_endText.color.r, _endText.color.g, _endText.color.b, 0);

        Tween.TMPAlpha(_endText.gameObject, 1, duration);
    }


    public void EndEndText(float duration)
    {
        _endText.color = new Color(_endText.color.r, _endText.color.g, _endText.color.b, 1);
        Tween.TMPAlpha(_endText.gameObject, 0, duration, TweenMode.Constant, () => _endText.gameObject.SetActive(false));
    }
}
