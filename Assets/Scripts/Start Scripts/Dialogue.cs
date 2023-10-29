using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
    [Tooltip("���� ����")]
    public string Contexts;

    [Tooltip("���� ��ư ����")]
    public string LeftButtonContexts;

    [Tooltip("������ ��ư ����")]
    public string RightButtonContexts;

    [Tooltip("�̺�Ʈ ��ȣ")]
    public string Number;

    [Tooltip("���� ��ư ��� ����")]
    public string LeftButtonOutput;

    [Tooltip("������ ��ư ��� ����")]
    public string RightButtonOutput;
}

