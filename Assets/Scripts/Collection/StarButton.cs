using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarButton : MonoBehaviour
{
    [Tooltip("UI�� ����ٴ� ������Ʈ")]
    [SerializeField] private Transform _targetTransform;

    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(_targetTransform.position);
    }
}
