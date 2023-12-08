using BT;
using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;

public class Event8_8 : StoryEvent
{
    private bool _clickEnable;
    [SerializeField] private GameObject _fish;
    private Coroutine _clickCoroutine;
    public override void EventStart(Action onComplate)
    {
        Tween.SpriteRendererAlpha(gameObject, 1, 1, TweenMode.Quadratic);

        _clickEnable = true;
        _clickCoroutine = StartCoroutine(ButtonClickEnable(onComplate));
    }

    public override void EventCancel(Action onComplate = null)
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);


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

                    // 버튼 클릭하면 강 정화
                    // 물고기 폴짝..
                    Tween.SpriteRendererAlpha(gameObject, 0, 0.5f, TweenMode.Quadratic);
                    Tween.SpriteRendererAlpha(_fish, 1, 1, TweenMode.Quadratic);
                    Vector3 targetPos = new Vector3(_fish.transform.position.x, _fish.transform.position.y, Camera.main.transform.position.z);
                    Tween.TransformMove(Camera.main.gameObject, (Camera.main.gameObject.transform.position + targetPos)/2, 3, TweenMode.Smootherstep, () =>
                    {
                        Tween.TransformMove(_fish.gameObject, _fish.transform.position + transform.up, 0.5f, TweenMode.Smootherstep, () =>
                        {
                            Tween.SpriteRendererAlpha(_fish, 0, 0.5f, TweenMode.Quadratic);
                            Tween.TransformMove(_fish.gameObject, _fish.transform.position - transform.up, 0.5f, TweenMode.Smootherstep, onComplate);
                        });
                    });
                }
            }

            yield return null;
        }
    }
}
