using Muks.Tween;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cooking
{
    public class UICookTimer : MonoBehaviour
    {
        [Header("Time")]
        [Tooltip("애니메이션 시작 시간 비율(ex. 시간제한은 60초, 비율은 0.5면 60 * 0.5 = 30초 후에 애니메이션 시작")]
        [SerializeField] private float _animeStartTimeMul;

        [Space]
        [Header("Objects")]
        [SerializeField] private GameObject _clock;

        [SerializeField] private GameObject _clockHands;

        [SerializeField] private GameObject _leftClockHead;

        [SerializeField] private GameObject _rightClockHead;

        private bool _isStarted;

        private bool _animeStarted;

        private float _timerValue;

        private float _currentTime;

        private Vector3 _clockTmpPos;

        private Action _onTimeoutd;


        public void Init()
        {
            _clockTmpPos = _clock.transform.position;
        }


        public void Update()
        {
            if (!_isStarted)
                return;

            if(!_animeStarted && _timerValue - (_timerValue * _animeStartTimeMul) < _currentTime)
            {
                _animeStarted = true;
                HeadAnime();
            }

            if (_currentTime < _timerValue)
            {
                _currentTime += Time.deltaTime;

                float angleZ = Mathf.Lerp(0, 360f, _currentTime / _timerValue);
                _clockHands.transform.rotation = Quaternion.Euler(0, 0, angleZ);
            }
            else
            {
                TimeOut();
            }

        }

        public void StartTimer(float value, Action onTimeoutd = null)
        {
            _timerValue = value;
            _currentTime = 0;
            _isStarted = true;
            _animeStarted = false;

            _clock.transform.position = _clockTmpPos;
            _clockHands.transform.eulerAngles = Vector3.zero;
            _leftClockHead.transform.eulerAngles = Vector3.zero;
            _rightClockHead.transform.eulerAngles = Vector3.zero;

            _onTimeoutd = onTimeoutd;
        }


        public void EndTimer()
        {
            Tween.Stop(_clock);
            Tween.Stop(_leftClockHead);
            Tween.Stop(_rightClockHead);

            _isStarted = false;
            _animeStarted = false;
        }


        private void TimeOut()
        {
            EndTimer();
            _onTimeoutd?.Invoke();
        }


        private void HeadAnime()
        {
            Tween.Stop(_clock);
            Tween.Stop(_leftClockHead);
            Tween.Stop(_rightClockHead);

            Vector3 targetRot = new Vector3(0, 0, 10);
            Tween.TransformRotate(_leftClockHead, targetRot, 0.1f, TweenMode.Constant).Loop(LoopType.Yoyo);
            Tween.TransformRotate(_rightClockHead, -targetRot, 0.1f, TweenMode.Constant).Loop(LoopType.Yoyo);

            Vector3 startPos = _clockTmpPos + new Vector3(-5, -5, 0);
            Vector3 targetPos = _clockTmpPos + new Vector3(5, 5, 0);
            _clockTmpPos = startPos;
            Tween.TransformMove(_clock, targetPos, 0.08f, TweenMode.Constant).Loop();
        }
    }

}
