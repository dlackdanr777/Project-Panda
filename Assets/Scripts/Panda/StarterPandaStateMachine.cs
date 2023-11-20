using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterPandaStateMachine : StateMachine
{


    protected StarterPanda _panda;


    //���ѻ��¸ӽ� ������
    //==========================================================================

    /// <summary> ���� ���¸� ���� </summary>
    public BaseState CurrentState { get; private set; }
     
    /// <summary> ��� ���� </summary>
    public BaseState IdleState { get; private set; }


    //==========================================================================

    public StarterPandaStateMachine(StarterPanda panda)
    {
        _panda = panda;
        StateInit();
    }


    private void StateInit()
    {
        IdleState = new IdleState(_panda, this);
        //�Ʒ��� �߰� ���°� �� ����� �⺻ ������ �ؾ��մϴ�.

        CurrentState = IdleState;

    }


    public override void OnStateUpdate()
    {
        CurrentState?.OnUpdate();
    }


    public override void OnStateFixedUpdate()
    {
        CurrentState?.OnFixedUpdate();
    }

}
