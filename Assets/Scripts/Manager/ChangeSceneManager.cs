using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;
using System;

public class ChangeSceneManager : SingletonHandler<ChangeSceneManager>
{
    [SerializeField] private Image _fadeIamge;
    [SerializeField] private float _fadeDuration;
    public void ChangeScene(float targetAlpha, Action onComplate = null)
    {
        _fadeIamge.gameObject.SetActive(true);
        Color color = _fadeIamge.color;
        color.a = Mathf.Abs(targetAlpha - 1);
        _fadeIamge.color = color;

        Tween.IamgeAlpha(_fadeIamge.gameObject, targetAlpha, _fadeDuration, TweenMode.Quadratic, onComplate);
    }

    public void ResetFadeIamge()
    {
        _fadeIamge.gameObject.SetActive(false);
    }
}
