using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public IState CurrentState { get; private set; } //현재 상태
    //기본 상태를 생성시에 설정하게 생성자 만들기
    public StateMachine(IState defaultState)
    {
        CurrentState = defaultState;
    }

    //외부에서 현재상태를 바꿔주는 부분
    public void SetState(IState state)
    {
        if(CurrentState == state)
        {
            Debug.Log("현재 이미 해당 상태");
            return;
        }
        CurrentState.OperateExit();
        CurrentState = state;
        CurrentState.OperateEnter();
    }

    //매프레임마다 호출
    public void DoOperateUpdate()
    {
        CurrentState.OperateUpdate();
    }
}
