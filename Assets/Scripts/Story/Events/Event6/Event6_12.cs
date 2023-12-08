using Muks.Tween;
using System;
using UnityEngine;

public class Event6_12 : StoryEvent
{
    public override void EventStart(Action onComplate)
    {
        // ÂûÄ¬ÂûÄ¬ È¿°ú
        Tween.SpriteRendererAlpha(gameObject, 1, 0.05f, TweenMode.Quadratic, () =>
        {
            Tween.SpriteRendererAlpha(gameObject, 0, 0.1f, TweenMode.Quadratic, () =>
            {
                Tween.SpriteRendererAlpha(gameObject, 1, 0.05f, TweenMode.Quadratic, () =>
                {
                    Tween.SpriteRendererAlpha(gameObject, 0, 0.1f, TweenMode.Quadratic, onComplate);
                });
            });
        });
    }

    public override void EventCancel(Action onComplate = null)
    {

    }
}
