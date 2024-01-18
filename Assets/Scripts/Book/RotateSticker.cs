using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotataeSticker : MonoBehaviour, IDragHandler
{
    [SerializeField] private GameObject _sticker;

    private Vector3 _startDragPosition;
    private float _startRotationZ;
    private bool _isDragging = false;

    private void Start()
    {
        _startRotationZ = _sticker.transform.localEulerAngles.z;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _startDragPosition = eventData.position;
        _isDragging = true;
        Debug.Log("드래그 시작");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging)
            return;
        Debug.Log("드래그 중");
        Vector3 currentMousePosition = eventData.position;
        Vector3 direction = currentMousePosition - _startDragPosition;

        // 각도 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Debug.Log(angle);

        // 시작할 때의 각도에 더해줌
        float newRotationZ = _startRotationZ + angle;

        // 회전 적용
        transform.rotation = Quaternion.Euler(0f, 0f, newRotationZ);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 끝");
        _isDragging = false;
    }
}
