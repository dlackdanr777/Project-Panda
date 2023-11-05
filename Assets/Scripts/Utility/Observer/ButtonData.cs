using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ButtonData
{
    public event Action<UnityAction> CallBack;

    private UnityAction _action;

    public UnityAction Action
    {
        get { return _action; }
        set
        {
            _action = value;
            CallBack?.Invoke(Action);
        }
    }
}
