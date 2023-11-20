using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Muks.DataBind;

/// <summary>
/// 상태 데이터를 저장하는 클래스
/// 성격 데이터 참조 및 함수 실행
/// </summary>
/// 
public class StateData
{
    //상태 이미지 변경 액션
    public Action<int> StateHandler;

    private enum PandaState
    {
        Happiness,
        Hunger,
        Depression,
        Irritation
    }
    private float[] _pandaStateValue = new float[4];
    private PandaState _currentPandaState { get; set; }
    private StateMachine stateMachine;

    //스테이트들을 보관
    private Dictionary<PandaState, IState> dicState = new Dictionary<PandaState, IState>();

    public void InitStateData()
    { 
        //상태 생성
        IState happiness = new StateHappiness();
        IState hunger = new StateHunger();
        IState depression = new StateDepression();
        IState irritation = new StateIrritation();

        for(int i = 0; i < 4; i++)
        {
            _pandaStateValue[i] = 50;
        }

        //키입력 등에 따라서 언제나 상태를 꺼내 쓸 수 있게 딕셔너리에 보관
        dicState.Add(PandaState.Happiness, happiness);
        dicState.Add(PandaState.Hunger, hunger);
        dicState.Add(PandaState.Depression, depression);
        dicState.Add(PandaState.Irritation, irritation);

        //기본상태를 행복으로 설정
        _currentPandaState = PandaState.Happiness;
        stateMachine = new StateMachine(happiness);
        StateHandler?.Invoke((int)_currentPandaState);
    }

    private void ChangeState()
    {
        StateHandler?.Invoke((int)_currentPandaState); //상태 이모티콘 이미지 변경 액션 실행

        stateMachine.DoOperateUpdate(); //매 프레임 실행해야하는 동작 호출
    }


    // 상태 변경되면 상태 체크
    public void CheckState()
    {
        if (IsHappiness())
        {
            _currentPandaState = PandaState.Happiness;
        }
        else if (IsHunger())
        {
            _currentPandaState = PandaState.Hunger;
        }
        else if (IsDepression())
        {
            _currentPandaState = PandaState.Depression;
        }
        else if (IsIrritation())
        {
            _currentPandaState = PandaState.Irritation;
        }
        else { return; }
        stateMachine.SetState(dicState[_currentPandaState]);
        ChangeState();
    }

    //(수정)상태 변경 조건에 따라 상태 변경되는 것으로 수정
    bool IsHappiness()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
            return true;
        else
            return false;
    }

    bool IsHunger()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            return true;
        else
            return false;
    }

    bool IsDepression()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
            return true;
        else
            return false;
    }

    bool IsIrritation()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
            return true;
        else
            return false;
    }
}
