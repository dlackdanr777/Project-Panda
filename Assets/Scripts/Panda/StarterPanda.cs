using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ��Ÿ�� �Ǵ� Ŭ���� </summary>
public class StarterPanda : Panda, IInteraction
{

    /// <summary> ��Ÿ�� �Ǵ� ���� �ӽ� </summary>
    private StarterPandaStateMachine _machine;

    private void Awake()
    {
        Init();
    }


    private void Update()
    {
        _machine?.OnStateUpdate();
    }


    private void FixedUpdate()
    {
        _machine?.OnStateFixedUpdate();
    }


    private void Init()
    {
        _machine = new StarterPandaStateMachine(this);
    }



    public void StartInteraction()
    {
    }

    public void UpdateInteraction()
    {
    }

    public void ExitInteraction()
    {
    }
}
