using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Tooltip("��ȭ ���̿� �̺�Ʈ�� �����ϴ� Ŭ����")]

public abstract class StoryEvent : MonoBehaviour
{

    [Tooltip("�̰� ���̸� �̺�Ʈ�� �����ٴ� ���Դϴ�. ")]
    public bool IsComplate;


    public abstract void EventStart(Action onComplate);

}
