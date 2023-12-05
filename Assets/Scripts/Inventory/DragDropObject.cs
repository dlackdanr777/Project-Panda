using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas _canvas;
    private GameObject _itemPf;
    private GameObject _currentItem;

    public void OnBeginDrag(PointerEventData eventData)
    {
        MoveTo(transform as RectTransform, eventData.position);

        Debug.Log(transform as RectTransform);  
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveTo(transform as RectTransform, eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    private void MoveTo(RectTransform rectItem, Vector2 mousePosition)
    {
        if(rectItem == null)
        {
            return;
        }
        Vector2 rectPosition = new();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, Camera.main, out rectPosition);
        rectItem.localPosition = rectPosition;
    }
}
