using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AreaImage : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 _startingPoint;
    private Vector2 _moveBegin;
    private Vector2 _moveOffset;

    public event Action OnPointerDownHandler;
    public event Action OnPointerUpHandler;
    public event Action OnDragHandler;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            _startingPoint = transform.position;
            _moveBegin = eventData.position;

            OnPointerDownHandler?.Invoke();
        }
    }

    // 드래그 : 마우스 커서 위치로 이동
     void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _moveOffset = eventData.position - _moveBegin;
            transform.position = _startingPoint + _moveOffset;

            OnDragHandler?.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _moveOffset = eventData.position - _moveBegin;
            transform.position = _startingPoint + _moveOffset;

            OnPointerUpHandler?.Invoke();
        }
    }
}
