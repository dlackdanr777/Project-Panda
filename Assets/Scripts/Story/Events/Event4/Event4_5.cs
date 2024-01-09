using Muks.Tween;
using System;
using UnityEngine;

public class Event4_5 : StoryEvent
{
    public override void EventStart(Action onComplate)
    {
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 2, TweenMode.Smootherstep, onComplate);
    }

    public override void EventCancel(Action onComplate = null)
    {
        Tween.Stop(Camera.main.gameObject);
    }
}
