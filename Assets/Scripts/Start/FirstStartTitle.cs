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
        [Tooltip("�̵� ������Ʈ")]
        public GameObject MoveObject;

        [Tooltip("�̵� �Ÿ�")]
        public Vector3 MovePosition;

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
