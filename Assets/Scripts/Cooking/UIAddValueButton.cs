using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Muks.Tween;

public class UIAddValueButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    public Button Button => _button;

    private bool _isUsabled;

    public void Start()
    {
        _button.onClick.AddListener(NotUsabledAnimation); 
    }

    public void CheckUsabled(bool isUsabled, UnityAction onClick)
    {
        _button.onClick.RemoveAllListeners();

        if (isUsabled)
        {
            _button.onClick.AddListener(onClick);
        }
        else
        {
            _button.onClick.AddListener(NotUsabledAnimation);
        }
    }


    private void NotUsabledAnimation()
    {

        Tween.Stop(_button.gameObject);

        Vector3 targetPos1 = _button.gameObject.transform.position + new Vector3(1f, 1, 0);
        Vector3 targetPos2 = _button.gameObject.transform.position - new Vector3(1f, 1, 0);

        Tween.TransformMove(_button.gameObject, targetPos1, 0.1f, TweenMode.Spike);
        Tween.TransformMove(_button.gameObject, targetPos2, 0.1f, TweenMode.Spike);
        Tween.TransformMove(_button.gameObject, targetPos1, 0.1f, TweenMode.Spike);
        Tween.TransformMove(_button.gameObject, targetPos2, 0.1f, TweenMode.Spike);
        Tween.TransformMove(_button.gameObject, targetPos1, 0.1f, TweenMode.Spike);
    }
}
