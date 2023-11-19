using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public IState CurrentState { get; private set; } //���� ����
    //�⺻ ���¸� �����ÿ� �����ϰ� ������ �����
    public StateMachine(IState defaultState)
    {
        CurrentState = defaultState;
    }

    //�ܺο��� ������¸� �ٲ��ִ� �κ�
    public void SetState(IState state)
    {
        if(CurrentState == state)
        {
            Debug.Log("���� �̹� �ش� ����");
            return;
        }
        CurrentState.OperateExit();
        CurrentState = state;
        CurrentState.OperateEnter();
    }

    //�������Ӹ��� ȣ��
    public void DoOperateUpdate()
    {
        CurrentState.OperateUpdate();
    }
}
