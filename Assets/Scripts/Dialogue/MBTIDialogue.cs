using System;
using UnityEngine;

[Serializable]
public class MBTIDialogue
{
    [Tooltip("질문 내용")]
    public string Contexts;

    [Tooltip("왼쪽 버튼 내용")]
    public string LeftButtonContexts;

    [Tooltip("오른쪽 버튼 내용")]
    public string RightButtonContexts;

    [Tooltip("이벤트 번호")]
    public string Number;

    [Tooltip("왼쪽 버튼 출력 문자")]
    public string LeftButtonOutput;

    [Tooltip("오른쪽 버튼 출력 문자")]
    public string RightButtonOutput;
}

