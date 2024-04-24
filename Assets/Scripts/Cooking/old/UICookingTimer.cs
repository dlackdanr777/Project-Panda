using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UICookingTimer : MonoBehaviour
{
    [SerializeField] private GameObject _clockHands;

    private UiCookingStart _uiCookingStart;

    private bool _isStarted;

    private float _timerValue;

    private float _currentTime;

    public void Init(UiCookingStart uiCookingStart)
    {
        _uiCookingStart = uiCookingStart;
    }

    public void Update()
    {
        if (!_isStarted)
            return;

        if(_currentTime < _timerValue)
        {
            _currentTime += Time.deltaTime;

            float angleZ = Mathf.Lerp(0, -360f, _currentTime / _timerValue);
            _clockHands.transform.rotation = Quaternion.Euler(0, 0, angleZ);
        }
        else
        {
            TimeOut();
            EndTimer();
        }

    }

    public void StartTimer(float value)
    {
        _timerValue = value;
        _currentTime = 0;
        _clockHands.transform.eulerAngles = new Vector3(0, 0, 0);
        _isStarted = true;
    }

    public void EndTimer()
    {
        _isStarted = false;
    }

    public void TimeOut()
    {
        _uiCookingStart.TimeOut();
    }
}
