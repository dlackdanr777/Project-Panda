using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private Canvas _canvas;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Transform _oldParent;
    private Vector3 _oldPosition;

    private void Awake()
    {
        _canvas = transform.root.GetComponent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _oldParent = transform.parent;
        _oldPosition = transform.position;
        _canvasGroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.parent = transform.root; //맨 위로 가도록
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        transform.parent = _oldParent;
        transform.position = _oldPosition;
        
    }
}
