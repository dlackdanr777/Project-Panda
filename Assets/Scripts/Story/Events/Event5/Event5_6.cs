using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;

public class Event5_6 : StoryEvent
{
    private bool _clickEnable;

    private Coroutine _clickCoroutine;

    [SerializeField] private GameObject _mapButton;
    public override void EventStart(Action onComplate)
    {
        _mapButton.SetActive(true);
        //�� �̵� ��ư ���� �̵��ϸ� �̺�Ʈ ����
        _clickEnable = true;
        _clickCoroutine = StartCoroutine(ButtonClickEnable(onComplate));
    }

    public override void EventCancel(Action onComplate = null)
    {
        _mapButton.SetActive(false);

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
                Debug.Log("mousepos: " + mousePos);
                Debug.Log("mapbutton: " + _mapButton.transform.position);
                RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.one, 10);

                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.transform.gameObject != _mapButton.gameObject)
                        continue;
                    Debug.Log("������ �̵�");
                    _clickEnable = false;
                    onComplate?.Invoke();
                }
            }

            yield return null;
        }
    }
}
