using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Timer : MonoBehaviour
{
    private int _currentHour;
    private float _currentMinute;

    [SerializeField] private GameObject _clockHands; // 시계 초침

    private void Start()
    {
        _currentHour = TimeManager.Instance.GameHour;
        _currentMinute = 0;

        float angleZ = Mathf.Lerp(0, 360f, _currentHour / 24f);
        _clockHands.transform.rotation = Quaternion.Euler(0, 0, angleZ);
    }

    private void Update()
    {
        // Hour 변경 시
        if(_currentHour != TimeManager.Instance.GameHour)
        {
            _currentHour = TimeManager.Instance.GameHour;
            _currentMinute = 0;

            float angleZ = Mathf.Lerp(0, 360f, _currentHour / 24f);
            _clockHands.transform.rotation = Quaternion.Euler(0, 0, angleZ);
        }
        else
        {
            _currentMinute += (Time.deltaTime / 60f);
            _currentMinute %= 60;
            float angleZ = Mathf.Lerp(0, 360f, (_currentHour + _currentMinute) / 24f);
            _clockHands.transform.rotation = Quaternion.Euler(0, 0, angleZ);
        }
    }
}
