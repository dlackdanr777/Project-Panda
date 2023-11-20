using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Muks.DataBind;

/// <summary>
/// ���� �����͸� �����ϴ� Ŭ����
/// ���� ������ ���� �� �Լ� ����
/// </summary>
/// 
public class StateData
{
    //���� �̹��� ���� �׼�
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

    //������Ʈ���� ����
    private Dictionary<PandaState, IState> dicState = new Dictionary<PandaState, IState>();

    public void InitStateData()
    { 
        //���� ����
        IState happiness = new StateHappiness();
        IState hunger = new StateHunger();
        IState depression = new StateDepression();
        IState irritation = new StateIrritation();

        for(int i = 0; i < 4; i++)
        {
            _pandaStateValue[i] = 50;
        }

        //Ű�Է� � ���� ������ ���¸� ���� �� �� �ְ� ��ųʸ��� ����
        dicState.Add(PandaState.Happiness, happiness);
        dicState.Add(PandaState.Hunger, hunger);
        dicState.Add(PandaState.Depression, depression);
        dicState.Add(PandaState.Irritation, irritation);

        //�⺻���¸� �ູ���� ����
        _currentPandaState = PandaState.Happiness;
        stateMachine = new StateMachine(happiness);
        StateHandler?.Invoke((int)_currentPandaState);
    }

    private void ChangeState()
    {
        StateHandler?.Invoke((int)_currentPandaState); //���� �̸�Ƽ�� �̹��� ���� �׼� ����

        stateMachine.DoOperateUpdate(); //�� ������ �����ؾ��ϴ� ���� ȣ��
    }


    // ���� ����Ǹ� ���� üũ
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

    //(����)���� ���� ���ǿ� ���� ���� ����Ǵ� ������ ����
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
