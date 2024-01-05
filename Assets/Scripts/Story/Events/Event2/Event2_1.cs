using System;
using UnityEngine;
using Muks.Tween;
using UnityEngine.UI;
using System.Collections;

public class Event2_1 : StoryEvent
{

    public override void EventStart(Action onComplate)
    {
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y + 1, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 3, TweenMode.Smootherstep);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 2, TweenMode.Smootherstep, onComplate);
    }

    public override void EventCancel(Action onComplate = null)
    {
        Tween.Stop(Camera.main.gameObject);
    }
}
