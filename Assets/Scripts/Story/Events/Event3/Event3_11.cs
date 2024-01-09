using Muks.Tween;
using System;
using UnityEngine;
using System.Collections;

public class Event3_11 : InteractionStoryEvent
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _uiShop;

    public override void EventStart(Action onComplate)
    {
        _nextActionHandler = onComplate;
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 3, TweenMode.Smootherstep);
        Tween.SpriteRendererAlpha(_spriteRenderer.gameObject, 1, 1, TweenMode.Quadratic, () =>
        {
            ShowFollowButton();
        });
    }

    public override void EventCancel(Action onComplate = null)
    {
        HideFollowButton();
        Tween.Stop(Camera.main.gameObject);
        Tween.Stop(_spriteRenderer.gameObject);

        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0);
        _uiShop.SetActive(false);
    }

    protected override void OnFollowButtonClicked()
    {
        HideFollowButton();
        _uiShop.SetActive(true);
        _nextActionHandler?.Invoke();
    }
}
