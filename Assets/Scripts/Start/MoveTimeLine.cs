using System;
using UnityEngine;
using Muks.Tween;

[Tooltip("������Ʈ �̵� Ÿ�Ӷ����� �����ϴ� Ŭ����")]
public class MoveTimeLine : StartClass
{
    
    [Serializable]
    public struct TimeLine
    {
        [Tooltip("�̵� ������Ʈ")]
        public GameObject Object;

        [Tooltip("�̵� �Ÿ�")]
        public Vector3 TargetPosition;

        [Tooltip("�̵� �ð�")]
        public float Duration;

        [Tooltip("�ִϸ��̼� Ŀ��")]
        public TweenMode TweenMode;
    }

    [Tooltip("�̵� ����, ������Ʈ, �Ÿ�, �ð��� ������ �� �ִ�.")]
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

        _isStart = true;
        Debug.Log(_isStart);
        for (int i = 0; i < _timeLines.Length; i++)
        {
            //������ ������ ��츸 �ݹ��Լ��� ����.
            if (i == _timeLines.Length - 1)
            {
                Tween.TransformMove(_timeLines[i].Object, _timeLines[i].TargetPosition, _timeLines[i].Duration, _timeLines[i].TweenMode, UIEnd);
                return;
            }
                
            Tween.TransformMove(_timeLines[i].Object, _timeLines[i].TargetPosition, _timeLines[i].Duration, _timeLines[i].TweenMode);
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