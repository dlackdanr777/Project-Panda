using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstStartTitle : StartList
{
    
    [Serializable]
    public struct TimeLine
    {
        [Tooltip("이동 오브젝트")]
        public GameObject MoveObject;

        [Tooltip("이동 거리")]
        public Vector3 MovePosition;

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
            StartCoroutine(MoveCamera());
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


    private IEnumerator MoveCamera()
    {   
        for(int i = 0, count = _timeLines.Length; i < count; i++)
        {
            Vector3 targetPos = _timeLines[i].MoveObject.transform.position;
            Vector3 destinationPos = targetPos + _timeLines[i].MovePosition;
            float timer = 0;

            while (timer < _timeLines[i].Duration)
            {
                timer += Time.deltaTime;

                float t = timer / _timeLines[i].Duration;
                t = t * t * (3f - 2f * t);

                _timeLines[i].MoveObject.transform.position = Vector3.Lerp(targetPos, destinationPos, t);
                
                yield return null;
            }
        }

        yield return new WaitForSeconds(3);
        UIEnd();
    }
}
