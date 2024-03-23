using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    [Tooltip("UI�� ����ٴ� ������Ʈ")]
    [SerializeField] private Transform _targetTrnsform;

    [Tooltip("���� ��ġ ����")]
    [SerializeField] private Vector3 _addPos;

    [Tooltip("�ش� ���丮ID�� �Ϸ� ������ ��� Ȱ��(������ ��� ������ Ȱ��)")]
    [SerializeField] private string _stroyId;

    private void Start()
    {
        if (_targetTrnsform == null)
            enabled = false;
    }

    private void Update()
    {
        //Vector3 screenPointPos = Camera.main.WorldToScreenPoint(_targetTrnsform.position + _addPos);
        transform.position = Camera.main.WorldToScreenPoint(_targetTrnsform.position + _addPos);
    }
}
