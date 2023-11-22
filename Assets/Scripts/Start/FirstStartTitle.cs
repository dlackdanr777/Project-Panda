using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Muks.Tween;

using Muks.DataBind;

public class FirstStartTitle : StartList
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
    }

    [Tooltip("�̵��� Ÿ��Ʋ")]
    [SerializeField] private TextMeshPro _startTitle;

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
        if (!_isStart)
        {
            //StartCoroutine(FirstScene());
            Tween.TransformMove(_timeLines[0].Object, _timeLines[0].TargetPosition, _timeLines[0].Duration, TweenMode.Smootherstep);
            Tween.TransformMove(_timeLines[1].Object, _timeLines[1].TargetPosition, _timeLines[1].Duration, TweenMode.Smootherstep);
            Tween.TransformMove(_timeLines[2].Object, _timeLines[2].TargetPosition, _timeLines[2].Duration, TweenMode.Smootherstep, UIEnd);

                _isStart = true;
            Debug.Log("����");
        }
        else
        {
            Debug.Log("�̹� ������ �Դϴ�.");
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
