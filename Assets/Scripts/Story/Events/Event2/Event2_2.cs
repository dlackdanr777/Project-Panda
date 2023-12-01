using System;
using UnityEngine;
using Muks.Tween;
using UnityEngine.UI;
using System.Collections;

public class Event2_2 : StoryEvent
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject[] _changedObj;

    private bool _clickEnable;
    private int _clickCount;
    private Coroutine _clickCoroutine;

    public override void EventStart(Action onComplate)
    {
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
        _clickCount = 0;

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

                foreach(RaycastHit2D hit in hits)
                {
                    if (hit.transform.gameObject != gameObject)
                        continue;
              
                    _clickCount++;
                    if (_clickCount < _changedObj.Length)
                    {
                        _changedObj[_clickCount - 1].SetActive(false);
                        _changedObj[_clickCount].SetActive(true);
                    }       
                }
            }

            if (5 <= _clickCount)
            {
                onComplate?.Invoke();
                _clickEnable = false;
                yield break;
            }

            yield return null;
        }
    }
}
