using Muks.Tween;
using System;
using UnityEngine;
using System.Collections;

public class Event3_11 : StoryEvent
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _uiShop;

    private bool _clickEnable;
    private Coroutine _clickCoroutine;
    public override void EventStart(Action onComplate)
    {
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 3, TweenMode.Smootherstep);
        Tween.SpriteRendererAlpha(_spriteRenderer.gameObject, 1, 1, TweenMode.Quadratic, () =>
        {
            _clickEnable = true;
            _clickCoroutine = StartCoroutine(ButtonClickEnable(onComplate));
        });
    }

    public override void EventCancel(Action onComplate = null)
    {
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0);
        _clickEnable = false;

        if (_clickCoroutine != null)
            StopCoroutine(_clickCoroutine);
        _uiShop.SetActive(false);
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

                    _uiShop.SetActive(true);
                    onComplate?.Invoke();
                    _clickEnable = false;
                    yield break;
                }
            }

            yield return null;
        }
    }
}
