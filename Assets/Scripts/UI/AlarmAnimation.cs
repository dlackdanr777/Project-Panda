using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AlarmAnimation : MonoBehaviour
{
    private Image _image;
    private TweenData _tweenData;


    private void Awake()
    {
        _image = GetComponent<Image>();
        _tweenData = Tween.IamgeAlpha(_image.gameObject, 0.4f, 2.5f, TweenMode.EaseInQuint);
        _tweenData.Loop(LoopType.Yoyo);
    }


    private void OnEnable()
    {
        _tweenData.ElapsedDuration = 0;
        _tweenData.IsRightMove = true;
    }

    private void OnDisable()
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1);
    }
}
