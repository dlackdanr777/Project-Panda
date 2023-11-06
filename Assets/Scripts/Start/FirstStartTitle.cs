using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Muks.Tween;

public class FirstStartTitle : StartList
{
    
    [Serializable]
    public struct TimeLine
    {
        [Tooltip("이동 오브젝트")]
        public GameObject Object;

        [Tooltip("이동 거리")]
        public Vector3 TargetPosition;

        [Tooltip("이동 시간")]
        public float Duration;
    }

    [Tooltip("이동할 타이틀")]
    [SerializeField] private TextMeshPro _startTitle;

    [Tooltip("이동 순서, 오브젝트, 거리, 시간을 설정할 수 있다.")]
    [SerializeField] private TimeLine[] _timeLines;

    private StartClassController _uiStart;
    private bool _isStart;

    public override void Init(StartClassController uiStart)
    {
        _uiStart = uiStart;
    }

    public override void UIStart()
    {
        if (!_isStart)
        {
            //StartCoroutine(FirstScene());
            Tween.Move(_timeLines[0].Object, _timeLines[0].TargetPosition, _timeLines[0].Duration, TweenMode.Smootherstep);
            Tween.Move(_timeLines[1].Object, _timeLines[1].TargetPosition, _timeLines[1].Duration, TweenMode.Smootherstep);
            Tween.Move(_timeLines[2].Object, _timeLines[2].TargetPosition, _timeLines[2].Duration, TweenMode.Smootherstep, UIEnd);
                _isStart = true;
            Debug.Log("시작");
        }
        else
        {
            Debug.Log("이미 실행중 입니다.");
        }
    }

    public override void UIUpdate()
    {
        
    }
    public override void UIEnd()
    {
        _uiStart.ChangeCurrentClass();
    }
}
