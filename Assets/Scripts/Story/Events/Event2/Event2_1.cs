using System;
using UnityEngine;
using Muks.Tween;
using UnityEngine.UI;
using System.Collections;

public class Event2_1 : StoryEvent
{

    public override void EventStart(Action onComplate)
    {
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos + transform.up, 3, TweenMode.Smootherstep);
        Tween.TransformMove(Camera.main.gameObject, targetPos + transform.up, 2, TweenMode.Smootherstep, onComplate);
    }

    public override void EventCancel(Action onComplate = null)
    {
    
    }
}
