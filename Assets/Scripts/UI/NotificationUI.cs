using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    [Tooltip("UI가 따라다닐 오브젝트")]
    [SerializeField] private Transform _targetTrnsform;

    [Tooltip("세부 위치 조정")]
    [SerializeField] private Vector3 _addPos;

    [Tooltip("해당 스토리ID가 완료 상태일 경우 활성(공백일 경우 무조건 활성)")]
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
