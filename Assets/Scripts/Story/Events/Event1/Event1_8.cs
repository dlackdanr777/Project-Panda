using System;
using UnityEngine;
using Muks.Tween;
using UnityEngine.UI;
using System.Collections;

public class Event1_8 : StoryEvent
{
    [SerializeField] private UINavigation _uiNav;
    private bool _clickEnable;

    private Coroutine _clickCoroutine;

    public override void EventStart(Action onComplate)
    {
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 3, TweenMode.Smootherstep, () =>
        {
            _clickEnable = true;
            _clickCoroutine = StartCoroutine(ButtonClickEnable(onComplate));
        });
    }

    public override void EventCancel(Action onComplate = null)
    {
        _clickEnable = false;

        if(_clickCoroutine != null)
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

                    _clickEnable = false;
                    Tween.TransformMove(gameObject, transform.position + transform.up, 2, TweenMode.Smootherstep, () =>
                    {
                        onComplate?.Invoke();
                    });
                }
            }
                
            yield return null;
        }
    }
}
