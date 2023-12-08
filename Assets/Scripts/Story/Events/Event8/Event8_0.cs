using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;

public class Event8_0 : StoryEvent
{
    private bool _clickEnable;
    [SerializeField] private GameObject _starterPanda;
    [SerializeField] private GameObject _Jiji;
    [SerializeField] private GameObject _fishingRods;
    [SerializeField] private GameObject _angler;


    private Coroutine _clickCoroutine;
    public override void EventStart(Action onComplate)
    {
        _starterPanda.SetActive(true);
        _Jiji.SetActive(true);
        _angler.SetActive(true);

        Tween.SpriteRendererAlpha(_fishingRods, 0, 1, TweenMode.Quadratic);
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 2f, TweenMode.Smootherstep, onComplate);
    }

    public override void EventCancel(Action onComplate = null)
    {
        _starterPanda.SetActive(false);
        _Jiji.SetActive(false);
        _angler.SetActive(false);

        SpriteRenderer fishingRodsRenderer = _fishingRods.GetComponent<SpriteRenderer>();
        fishingRodsRenderer.color = new Color(fishingRodsRenderer.color.r, fishingRodsRenderer.color.g, fishingRodsRenderer.color.b, 1);

        if (_clickCoroutine != null)
            StopCoroutine(_clickCoroutine);
    }
}
