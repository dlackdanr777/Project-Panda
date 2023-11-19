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

    public void ChangeState()
    {
        //(����)���� ����(Ű�Է����� ������ ���߿� ���� ���� ���ǿ� ���� ���� ����Ǵ� ������ ����)
        KeyboardInput();

        StateHandler?.Invoke((int)_currentPandaState); //���� �̸�Ƽ�� �̹��� ���� �׼� ����

        stateMachine.DoOperateUpdate(); //�� ������ �����ؾ��ϴ� ���� ȣ��
    }

    //(����)Ű���� �Է¹޾� ���� ����
    void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _currentPandaState = PandaState.Happiness;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentPandaState = PandaState.Hunger;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentPandaState = PandaState.Depression;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _currentPandaState = PandaState.Irritation;
        }
        else { return; }
        stateMachine.SetState(dicState[_currentPandaState]);
    }
}
