using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;

public class Event8_1 : StoryEvent
{
    private bool _clickEnable;

    [SerializeField] private GameObject _fishingButton;


    private Coroutine _clickCoroutine;
    public override void EventStart(Action onComplate)
    {
        Tween.SpriteRendererAlpha(_fishingButton, 1, 0.7f, TweenMode.Quadratic, () =>
        {
            _clickEnable = true;
            _clickCoroutine = StartCoroutine(ButtonClickEnable(onComplate));
        });
    }

    public override void EventCancel(Action onComplate = null)
    {
        SpriteRenderer fishButtonRenderer = _fishingButton.GetComponent<SpriteRenderer>();
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
                    if (hit.transform.gameObject != _fishingButton.gameObject)
                        continue;

                    // 낚시버튼 클릭
                    _clickEnable = false;
                    Tween.SpriteRendererAlpha(_fishingButton, 0, 1, TweenMode.Quadratic, onComplate);
                }
            }

            yield return null;
        }
    }
}
