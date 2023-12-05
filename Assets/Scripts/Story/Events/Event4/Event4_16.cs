using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;

public class Event4_16 : StoryEvent
{
    [SerializeField] private GameObject _shopYoungerBrother;
    //[SerializeField] private UINavigation _uiNav;
    private bool _clickEnable;
    private bool _isSpriteRendererAlphaOn;
    private float _targetAlpha;

    private Coroutine _clickCoroutine;
    public override void EventStart(Action onComplate)
    {
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        Tween.SpriteRendererAlpha(_shopYoungerBrother, 0, 2, TweenMode.Quadratic, () =>
        {
            Tween.TransformMove(Camera.main.gameObject, targetPos, 2, TweenMode.Smootherstep);
            Tween.SpriteRendererAlpha(gameObject, 1, 2, TweenMode.Quadratic);
            _clickEnable = true;
            _clickCoroutine = StartCoroutine(ButtonClickEnable(onComplate));
        });
    }

    public override void EventCancel(Action onComplate = null)
    {
        SpriteRenderer shopPandaRenderer = gameObject.GetComponent<SpriteRenderer>();
        shopPandaRenderer.color = new Color(shopPandaRenderer.color.r, shopPandaRenderer.color.g, shopPandaRenderer.color.b, 1);
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
        _clickEnable = false;

        if (_clickCoroutine != null)
            StopCoroutine(_clickCoroutine);
    }

    private IEnumerator ButtonClickEnable(Action onComplate)
    {
        while (_clickEnable)
        {
            //우편함 깜박임
            if (!_isSpriteRendererAlphaOn)
            {
                _isSpriteRendererAlphaOn = true;
                Tween.SpriteRendererAlpha(gameObject, _targetAlpha, 1, TweenMode.Quadratic, () =>
                {
                    _isSpriteRendererAlphaOn = false;
                    if(_targetAlpha == 0)
                        _targetAlpha = 1;
                    else
                        _targetAlpha = 0;
                });
            }
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.one, 10);

                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.transform.gameObject != gameObject)
                        continue;


                    //편지함 열어서 상점 형의 편지 띄워주기 + 선물 밤양갱 2개 받기 후 이벤트 종료
                    _clickEnable = false;
                    onComplate?.Invoke();
                }
            }

            yield return null;
        }
    }
}
