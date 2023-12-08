using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Event8_16 : StoryEvent
{
    [SerializeField] private GameObject _bait;
    [SerializeField] private GameObject _starterPanda;
    private bool _clickEnable;

    private Coroutine _clickCoroutine;
    public override void EventStart(Action onComplate)
    {
        Vector3 targetPos = new Vector3(_starterPanda.transform.position.x, _starterPanda.transform.position.y, _starterPanda.transform.position.z);
        Tween.SpriteRendererAlpha(_bait, 1, 0.5f, TweenMode.Quadratic);
        Tween.TransformMove(_bait, targetPos, 1.5f, TweenMode.Smootherstep, () =>
        {
            Tween.SpriteRendererAlpha(_bait, 0, 0.5f, TweenMode.Quadratic);
            Tween.SpriteRendererAlpha(gameObject, 1, 1, TweenMode.Quadratic);
        });
        _clickEnable = true;
        _clickCoroutine = StartCoroutine(ButtonClickEnable(onComplate));
    }

    public override void EventCancel(Action onComplate = null)
    {
        SpriteRenderer fishButtonRenderer = base.gameObject.GetComponent<SpriteRenderer>();
        fishButtonRenderer.color = new Color(fishButtonRenderer.color.r, fishButtonRenderer.color.g, fishButtonRenderer.color.b, 0);
        _clickEnable = false;

        if (_clickCoroutine != null)
            StopCoroutine(_clickCoroutine);
    }

    private IEnumerator ButtonClickEnable(Action onComplate)
    {
        while (_clickEnable)
        {

            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.one, 10);

                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.transform.gameObject != gameObject.gameObject)
                        continue;

                    _clickEnable = false;
                    Tween.SpriteRendererAlpha(gameObject, 0, 1, TweenMode.Quadratic, onComplate);
                }
            }

            yield return null;
        }
    }
}