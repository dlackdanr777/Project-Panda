using System;
using UnityEngine;
using Muks.Tween;

public class Event1_0 : StoryEvent
{

    public override void EventStart(Action onComplate)
    {
        Tween.SpriteRendererAlpha(gameObject, 1, 2, TweenMode.Quadratic, onComplate);
    }

}
