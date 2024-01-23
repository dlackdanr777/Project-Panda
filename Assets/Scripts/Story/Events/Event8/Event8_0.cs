using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;

public class Event8_0 : StoryEvent
{
    [SerializeField] private GameObject _starterPanda;
    [SerializeField] private GameObject _Jiji;
    [SerializeField] private GameObject _angler;

    public override void EventStart(Action onComplate)
    {
        _starterPanda.SetActive(true);
        _Jiji.SetActive(true);
        _angler.SetActive(true);

        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 2f, TweenMode.Smootherstep, onComplate);
    }

    public override void EventCancel(Action onComplate = null)
    {
        _starterPanda.SetActive(false);
        _Jiji.SetActive(false);
        _angler.SetActive(false);

    }
}
