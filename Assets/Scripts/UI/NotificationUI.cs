using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    [Tooltip("UI가 따라다닐 오브젝트")]
    [SerializeField] private Transform _targetTrnsform;

    [Tooltip("세부 위치 조정")]
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
