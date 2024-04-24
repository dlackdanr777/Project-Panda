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
    [Tooltip("ī�޶� ������, ��ȣ�ۿ��� �������ΰ� Ǯ���ΰ�?")]
    [SerializeField] private bool _unlockCamera;
    public bool UnlockCamera => _unlockCamera;

    protected UINavigation _uiNav;

    [HideInInspector] public RectTransform RectTransform;

    public virtual void Init(UINavigation uiNav)
    {
        _uiNav = uiNav;
        RectTransform = GetComponent<RectTransform>();
    }


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
