using System;
using UnityEngine;

[Serializable]
public class MBTIDialogue
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

