using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Tooltip("대화 사이에 이벤트를 실행하는 클래스")]

public abstract class StoryEvent : MonoBehaviour
{

    [Tooltip("이게 참이면 이벤트가 끝났다는 것입니다. ")]
    public bool IsComplate;


    //해당 이벤트가 시작됬을때 실행될 함수
    public abstract void EventStart(Action onComplate);

    //해당 이벤트가 캔슬됬을때 실행될 함수
    public abstract void EventCancel(Action onComplate = null);

}
