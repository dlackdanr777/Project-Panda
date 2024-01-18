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
        Debug.Log("�巡�� ����");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging)
            return;
        Debug.Log("�巡�� ��");
        Vector3 currentMousePosition = eventData.position;
        Vector3 direction = currentMousePosition - _startDragPosition;

        // ���� ���
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Debug.Log(angle);

        // ������ ���� ������ ������
        float newRotationZ = _startRotationZ + angle;

        // ȸ�� ����
        transform.rotation = Quaternion.Euler(0f, 0f, newRotationZ);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("�巡�� ��");
        _isDragging = false;
    }
}
