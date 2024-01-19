using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveSticker : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private GameObject _sticker;
    [SerializeField] private bool _isRight;

    private RectTransform _rectTransform;
    private Vector2 _originalSize;
    private Vector3 _startScale;
    private Vector3 _startDragPosition;
    private bool _isDragging = false;

    private void Start()
    {
        _rectTransform = _sticker.GetComponent<RectTransform>();
        _originalSize = _rectTransform.sizeDelta;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startDragPosition = eventData.position;
        _startScale = _rectTransform.localScale;
        _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging)
            return;
        
        Vector3 currentMousePosition = eventData.position;
        Vector3 direction = currentMousePosition - _startDragPosition;

        // 스케일 계산
        float scaleX;
        float scaleY;
        if (_isRight)
        {
            scaleX = Mathf.Clamp(_startScale.x + direction.x * 0.01f, 0.1f, 4f);
            scaleY = Mathf.Clamp(_startScale.y + direction.y * 0.01f, 0.1f, 4f);
        }
        else
        {
            scaleX = Mathf.Clamp(_startScale.x - direction.x * 0.01f, 0.1f, 4f);
            scaleY = Mathf.Clamp(_startScale.y - direction.y * 0.01f, 0.1f, 4f);
        }

        // 비율에 맞춰 스케일 조정
        float aspectRatio = _originalSize.x / _originalSize.y;
        if (direction.x != 0)
            scaleY = scaleX / aspectRatio;
        else if (direction.y != 0)
            scaleX = scaleY * aspectRatio;

        // 스케일 적용
        _rectTransform.localScale = new Vector3(scaleX, scaleY, 1f);
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
    }
}
