using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;

public class Event6_15 : StoryEvent
{
    private bool _clickEnable;
    private Coroutine _clickCoroutine;
    [SerializeField] private GameObject _flash;

    public override void EventStart(Action onComplate)
    {
        // 카메라 버튼 활성화
        Tween.SpriteRendererAlpha(gameObject, 1, 1, TweenMode.Quadratic, () =>
        {
            _clickEnable = true;
            _clickCoroutine = StartCoroutine(ButtonClickEnable(onComplate));
        });
    }

    public override void EventCancel(Action onComplate = null)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);

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
                    if (hit.transform.gameObject != gameObject)
                        continue;
                    _clickEnable = false;
                    Tween.SpriteRendererAlpha(gameObject, 0, 1, TweenMode.Quadratic);
                    // 플래쉬 효과
                    Tween.IamgeAlpha(_flash, 1, 0.1f, TweenMode.Quadratic, () =>
                    {
                        Tween.IamgeAlpha(_flash, 0, 0.2f, TweenMode.Quadratic, onComplate);
                    });
                }
            }

            yield return null;
        }
    }
}
