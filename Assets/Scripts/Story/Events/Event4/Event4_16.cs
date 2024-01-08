using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;

public class Event4_16 : InteractionStoryEvent
{
    [SerializeField] private GameObject _shopYoungerBrother;
    private bool _isSpriteRendererAlphaOn;
    private float _targetAlpha;

    public override void EventStart(Action onComplate)
    {
        _nextActionHandler = onComplate;
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y + 1, Camera.main.transform.position.z);


        Tween.TransformMove(Camera.main.gameObject, targetPos, 2, TweenMode.Smootherstep, () =>
        {
            ShowFollowButton();
            //StartCoroutine(ButtonEvent());
        });
    }

    public override void EventCancel(Action onComplate = null)
    {
        HideFollowButton();
        //StopCoroutine(ButtonEvent());
    }

   /* private IEnumerator ButtonEvent()
    {
        while (true)
        {
            if (!_isSpriteRendererAlphaOn)
            {
                _isSpriteRendererAlphaOn = true;
                Tween.IamgeAlpha(FollowButton.gameObject, _targetAlpha, 1, TweenMode.Quadratic, () =>
                {
                    _isSpriteRendererAlphaOn = false;
                    if (_targetAlpha == 0)
                        _targetAlpha = 1;
                    else
                        _targetAlpha = 0;
                });
            }
            
            yield return null;
        }
    }*/

    protected override void OnFollowButtonClicked()
    {
        _nextActionHandler?.Invoke();

        HideFollowButton();
        //StopCoroutine(ButtonEvent());
    }
}
