using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;

public class Event5_4 : StoryEvent
{
    private bool _clickEnable;

    private Coroutine _clickCoroutine;
    public override void EventStart(Action onComplate)
    {
        //���ڸ�ä ���� �� �̺�Ʈ ����
        Tween.SpriteRendererAlpha(gameObject, 1, 2, TweenMode.Quadratic);
        _clickEnable = true;
        _clickCoroutine = StartCoroutine(ButtonClickEnable(onComplate));
    }

    public override void EventCancel(Action onComplate = null)
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
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

                    // ���ڸ�ä ����
                    BuyDragonflyNet(onComplate);
                    _clickEnable = false;
                    
                }
            }

            yield return null;
        }
    }
    private void BuyDragonflyNet(Action onComplate)
    {
        //���� ä�� Ȱ��ȭ?
        Tween.SpriteRendererAlpha(gameObject, 0, 2, TweenMode.Quadratic, onComplate);
    }
}
