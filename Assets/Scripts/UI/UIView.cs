using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum VisibleState
{
    Disappeared, // �����
    Disappearing, //������� ��
    Appeared, //��Ÿ��
    Appearing, //��Ÿ������
}

public abstract class UIView : MonoBehaviour
{


    /// <summary>
    /// �̰��� Appeared�϶� Hide���� ����,
    /// �̰��� Disappeared�϶� Show���� ����
    /// </summary>
    public VisibleState VisibleState;

    /// <summary>
    /// UI�� �ҷ����� �ݹ�Ǵ� �Լ�
    /// </summary>
    public abstract void Show();


    /// <summary>
    /// UI�� ���� �ݹ�Ǵ� �Լ�
    /// </summary>
    public abstract void Hide();
}
