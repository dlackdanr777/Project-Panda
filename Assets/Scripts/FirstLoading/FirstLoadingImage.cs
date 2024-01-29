using Muks.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstLoadingImage : MonoBehaviour
{
    [SerializeField] private Image _bgImage;

    [SerializeField] private Image _animeImage;

    private Action _onComplete;


    public void Init(Action onComplete)
    {
        _onComplete = onComplete;
        _bgImage.color = new Color(_bgImage.color.r, _bgImage.color.g, _bgImage.color.b, 0);
        _animeImage.color = new Color(_animeImage.color.r, _animeImage.color.g, _animeImage.color.b, 0);
        _bgImage.gameObject.SetActive(false);
    }


    public void Loading(float fadeDuration)
    {
        _bgImage.gameObject.SetActive(true);

        Tween.IamgeAlpha(_animeImage.gameObject, 1, fadeDuration, TweenMode.Constant);
        Tween.IamgeAlpha(_bgImage.gameObject, 1, fadeDuration, TweenMode.Constant, () =>
        {

            Tween.TransformScale(_bgImage.gameObject, new Vector3(1, 1, 1), 3, TweenMode.Constant, () =>
            {

                Tween.IamgeAlpha(_bgImage.gameObject, 0, fadeDuration, TweenMode.Constant, () =>
                {
                    _bgImage.gameObject.SetActive(false);
                });

                Tween.IamgeAlpha(_animeImage.gameObject, 0, fadeDuration, TweenMode.Constant, () =>
                {
                    _bgImage.gameObject.SetActive(false);
                    _onComplete?.Invoke();
                });

            });

        });      
    }
}
