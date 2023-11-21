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
    public Action<float, float, Action> UIAlphaHandler;

    /// <summary>����</summary>
    protected string _mbtiData;

    /// <summary>ģ�е�</summary>
    protected int _intimacy;

    /// <summary>�ູ��</summary>
    [SerializeField]
    [Range(-10, 10)] protected float _happiness;
    /// <summary>���� �ູ��</summary>
    protected float _lastHappiness;

    //�Ʒ��� ���� ����, ģ�е� ���� �Լ��� �߻��Լ��� �ۼ��Ͻø� �˴ϴ�.
    public abstract void ChangeIntimacy(int changeIntimacy);

    public abstract void SetPreference(string mbti);
}
