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
        [Tooltip("�̵� ������Ʈ")]
        public GameObject MoveObject;

        [Tooltip("�̵� �Ÿ�")]
        public Vector3 MovePosition;

        [Tooltip("�̵� �ð�")]
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
