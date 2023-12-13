using Muks.Tween;
using System;
using UnityEngine;
using System.Collections;

public class Event3_13 : StoryEvent
{
    [SerializeField] private GameObject _uiShop;
    [SerializeField] private GameObject _exitShop;
    [SerializeField] private GameObject _shop;
    [SerializeField] private GameObject _bambooProduction;
    [SerializeField] private GameObject _uiDiary;

    private bool _clickEnable;
    private Coroutine _clickCoroutine;
    public override void EventStart(Action onComplate)
    {
        _clickEnable = true;
        _clickCoroutine = StartCoroutine(ButtonClickEnable(onComplate));
    }

    public override void EventCancel(Action onComplate = null)
    {
        _clickEnable = false;

        if (_clickCoroutine != null)
            StopCoroutine(_clickCoroutine);
        _uiShop.SetActive(true);
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
                    if (hit.transform.gameObject != _exitShop.gameObject)
                        continue;
                    ClickExitShop(onComplate);
                    yield break;
                }
            }

            yield return null;
        }
    }

    private void ClickExitShop(Action onComplate)
    {
        _uiShop.SetActive(false);
        onComplate?.Invoke();
        _clickEnable = false;
        _bambooProduction.SetActive(true);
        _shop.SetActive(true);
        _uiDiary.SetActive(true);
        //일기장에 첫 만남 기록 추가
    }
}
