using System;
using UnityEngine;
using Muks.Tween;

[Tooltip("오브젝트 이동 타임라인을 설정하는 클래스")]
public class MoveTimeLine : StartList
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

        [Tooltip("애니메이션 커브")]
        public TweenMode TweenMode;
    }

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
        if (_isStart)
            return;

        for (int i = 0; i < _timeLines.Length; i++)
        {
            //마지막 순번의 경우만 콜백함수를 쓴다.
            if (i == _timeLines.Length - 1)
            {
                Tween.TransformMove(_timeLines[i].Object, _timeLines[i].TargetPosition, _timeLines[i].Duration, _timeLines[i].TweenMode, UIEnd);
                continue;
            }
                
            Tween.TransformMove(_timeLines[i].Object, _timeLines[i].TargetPosition, _timeLines[i].Duration, _timeLines[i].TweenMode);
        }

        _isStart = true;
    }

    public override void UIUpdate()
    {
        
    }
    public override void UIEnd()
    {
        _uiStart.ChangeCurrentClass();
    }
}
