using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>NPC �Ǵ�, ��Ÿ�� �Ǵ��� �θ� Ŭ����</summary>
public abstract class Panda : MonoBehaviour
{
    [SerializeField]
    protected UIPanda _uiPanda;
    protected bool _isUISetActive;

    //���� �̹��� ���� �׼�
    public Action<int> StateHandler;

    /// <summary>����</summary>
    protected string _mbtiData;

    /// <summary>ģ�е�</summary>
    protected int _intimacy;

    /// <summary>�ູ��</summary>
    protected int _happiness;

    //�Ʒ��� ���� ����, ģ�е� ���� �Լ��� �߻��Լ��� �ۼ��Ͻø� �˴ϴ�.
    public abstract void AddIntimacy();
    public abstract void SubIntimacy();
}
