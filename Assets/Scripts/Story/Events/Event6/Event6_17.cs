using BT;
using Muks.Tween;
using System;
using UnityEngine;

/// <summary>
/// 도망가는 파파라치들 </summary>
public class Event6_17 : StoryEvent
{
    [SerializeField] private GameObject _darkPanda;
    [SerializeField] private GameObject _paparazzi;
    public override void EventStart(Action onComplate)
    {
        Vector3 targetPos = new Vector3(_darkPanda.transform.position.x, _darkPanda.transform.position.y, Camera.main.transform.position.z);        
        Tween.SpriteRendererAlpha(_paparazzi.gameObject, 0, 2, TweenMode.Quadratic, () =>
        {
            Tween.TransformMove(Camera.main.gameObject, targetPos, 3, TweenMode.Smootherstep, onComplate);
        });
    }

    public override void EventCancel(Action onComplate = null)
    {
        SpriteRenderer renderer = _paparazzi.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
    }
}
