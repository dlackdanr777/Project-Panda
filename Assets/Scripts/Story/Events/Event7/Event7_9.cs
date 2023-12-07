using BT;
using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;

public class Event7_9 : StoryEvent
{
    private bool _clickEnable;
    [SerializeField] private GameObject _starterPanda;
    [SerializeField] private GameObject _Jiji;
    [SerializeField] private GameObject _fishingRods;
    [SerializeField] private GameObject _angler;
    [SerializeField] private GameObject _fishingButton;

    private Coroutine _clickCoroutine;
    public override void EventStart(Action onComplate)
    {
        _starterPanda.SetActive(true);
        _Jiji.SetActive(true);
        _angler.SetActive(true);

        Tween.SpriteRendererAlpha(_fishingRods, 0, 1, TweenMode.Quadratic);
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 2f, TweenMode.Smootherstep, () =>
        {
            Tween.SpriteRendererAlpha(_fishingButton, 1, 0.7f, TweenMode.Quadratic);
            _clickEnable = true;
            _clickCoroutine = StartCoroutine(ButtonClickEnable(onComplate));
        });
    }

    public override void EventCancel(Action onComplate = null)
    {
        _starterPanda.SetActive(false);
        _Jiji.SetActive(false);
        _angler.SetActive(false);

        SpriteRenderer fishingRodsRenderer = _fishingRods.GetComponent<SpriteRenderer>();
        fishingRodsRenderer.color = new Color(fishingRodsRenderer.color.r, fishingRodsRenderer.color.g, fishingRodsRenderer.color.b, 1);
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

                    // ���ù�ư Ŭ��
                    _clickEnable = false;
                    Tween.SpriteRendererAlpha(_fishingButton, 0, 1, TweenMode.Quadratic, onComplate);          
                }
            }

            yield return null;
        }
    }
}
