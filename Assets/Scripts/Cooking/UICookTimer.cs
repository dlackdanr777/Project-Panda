using Muks.Tween;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cooking
{
    public class UICookTimer : MonoBehaviour
    { 
        [SerializeField] private GameObject _clockHands;

        [SerializeField] private GameObject _leftClockHead;

        [SerializeField] private GameObject _rightClockHead;

        private bool _isStarted; 

        private float _timerValue;

        private float _currentTime;

        private Action _onTimeoutd;


        public void Update()
        {
            if (!_isStarted)
                return;

            if (_currentTime < _timerValue)
            {
                _currentTime += Time.deltaTime;

                float angleZ = Mathf.Lerp(0, 360f, _currentTime / _timerValue);
                _clockHands.transform.rotation = Quaternion.Euler(0, 0, angleZ);
            }
            else
            {
                TimeOut();
                EndTimer();
            }

        }

        public void StartTimer(float value, Action onTimeoutd = null)
        {
            _timerValue = value;
            _currentTime = 0;
            _clockHands.transform.eulerAngles = new Vector3(0, 0, 0);
            _isStarted = true;

            _onTimeoutd = onTimeoutd;
        }


        public void EndTimer()
        {
            _isStarted = false;
        }


        private void TimeOut()
        {
            _onTimeoutd?.Invoke();
        }


        private void HeadAnime()
        {
            Tween.Stop(_leftClockHead);
            Tween.Stop(_rightClockHead);

            Vector3 targetRot = new Vector3(0, 0, 10);
            Tween.TransformRotate(_leftClockHead, targetRot, 0.1f, TweenMode.Constant).Loop(LoopType.Yoyo);
            Tween.TransformRotate(_rightClockHead, -targetRot, 0.1f, TweenMode.Constant).Loop(LoopType.Yoyo);
        }
    }

}
