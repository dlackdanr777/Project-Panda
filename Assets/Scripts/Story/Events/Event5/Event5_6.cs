using System;
using System.Collections;
using UnityEngine;

public class Event5_6 : StoryEvent
{
    private bool _clickEnable;
    private Coroutine _clickCoroutine;

    [SerializeField] private GameObject _mapButton;
    [SerializeField] private GameObject _mapImage;
    public override void EventStart(Action onComplate)
    {
        _mapButton.SetActive(true);
        //맵 이동 버튼 눌러 이동하면 이벤트 종료
        _clickEnable = true;
        _clickCoroutine = StartCoroutine(ButtonClickEnable(onComplate));
    }

    public override void EventCancel(Action onComplate = null)
    {
        _mapButton.SetActive(false);

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
                Vector2 mousePos = Input.mousePosition;
                RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.one, 10);

                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.transform.gameObject != _mapImage.gameObject)
                        continue;
                    Debug.Log("숲으로 이동");
                    _clickEnable = false;
                    onComplate?.Invoke();
                }
            }

            yield return null;
        }
    }
}
