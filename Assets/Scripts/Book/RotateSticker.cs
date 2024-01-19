using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotataeSticker : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject _sticker;
    [SerializeField] private float _rotationSpeed = 1f;

    private Vector3 _startDragPosition;
    private float _startRotationZ;
    private bool _isDragging = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startRotationZ = _sticker.transform.localEulerAngles.z;
        _startDragPosition = eventData.position;
        _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging)
            return;

        Vector3 currentMousePosition = eventData.position;
        Vector3 direction = currentMousePosition - _startDragPosition;

        // ���� ���
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //ȸ�� �ӵ� ����
        float rotationAmount = angle * _rotationSpeed;

        // ������ ���� ������ ������
        float newRotationZ = _startRotationZ + rotationAmount;

        // ȸ�� ���� ���� (0������ 360�� ���̷� ����)
        newRotationZ = Mathf.Repeat(newRotationZ, 360f);

        // ȸ�� ����
        _sticker.transform.rotation = Quaternion.Euler(0f, 0f, newRotationZ);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
    }
}
