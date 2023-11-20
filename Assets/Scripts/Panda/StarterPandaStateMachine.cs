using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterPandaStateMachine : StateMachine
{


    protected StarterPanda _panda;


    //유한상태머신 변수들
    //==========================================================================

    /// <summary> 현재 상태를 저장 </summary>
    public BaseState CurrentState { get; private set; }
     
    /// <summary> 대기 상태 </summary>
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
        //아래에 추가 상태가 더 생길시 기본 세팅을 해야합니다.

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
