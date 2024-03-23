using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    [Tooltip("UI�� ����ٴ� ������Ʈ")]
    [SerializeField] private Transform _targetTrnsform;

    [Tooltip("���� ��ġ ����")]
    [SerializeField] private Vector3 _addPos;


    private void Start()
    {
        if (_targetTrnsform == null)
            enabled = false;

    }


    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(_targetTrnsform.position + _addPos);
    }

}
