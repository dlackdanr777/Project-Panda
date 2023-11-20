using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 스타터 판다 클래스 </summary>
public class StarterPanda : Panda, IInteraction
{

    /// <summary> 스타터 판다 상태 머신 </summary>
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
