using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstStartTitle : UIStartList
{
    [Serializable]
    public struct TimeLine
    {
        [Tooltip("이동 오브젝트")]
        public GameObject MoveObject;

        [Tooltip("이동 거리")]
        public Vector3 MovePosition;

        [Tooltip("이동 시간")]
        public float MoveTime;
    }

    [SerializeField] private TextMeshPro _startTitle;
    [SerializeField] private TimeLine[] _timeLines;

    private UIStart _uiStart;
    private bool _isStart;

    public override void Init(UIStart uiStart)
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
        _uiStart.UIEnd();
    }


    private IEnumerator MoveCamera()
    {   
        for(int i = 0, count = _timeLines.Length; i < count; i++)
        {
            Vector3 targetPos = _timeLines[i].MoveObject.transform.position;
            Vector3 destinationPos = targetPos + _timeLines[i].MovePosition;
            float timer = 0;

            while (timer < _timeLines[i].MoveTime)
            {
                _timeLines[i].MoveObject.transform.position = Vector3.Lerp(targetPos, destinationPos, timer / _timeLines[i].MoveTime);
                timer += Time.deltaTime;
                yield return null;
            }
        }

        yield return new WaitForSeconds(3);
        UIEnd();
    }
}
