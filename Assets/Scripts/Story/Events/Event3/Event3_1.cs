using Muks.Tween;
using System;
using UnityEngine;

public class Event3_1 : StoryEvent
{
    public override void EventStart(Action onComplate)
    {
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 3, TweenMode.Smootherstep, onComplate);
    }

    public override void EventCancel(Action onComplate = null)
    {
        
    }
}
