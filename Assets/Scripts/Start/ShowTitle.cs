using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShowTitle : StartClass
{
    
    [SerializeField] private UIShowTitle _uiShowTitle;

    private StartClassController _startClass;

    private bool _isStart;

    public event Action OnStartHandler;


    public override void Init(StartClassController startClass)
    {
        _startClass = startClass;
        _uiShowTitle.Init(this);
        _uiShowTitle.OnButtonClickHandler += UIEnd;
    }

    public override void UIEnd()
    {
        _startClass.ChangeCurrentClass();
    }

    public override void UIStart()
    {
        if (_isStart)
            return;

            _isStart = true;
        OnStartHandler?.Invoke();
    }

    public override void UIUpdate()
    {

    }
}
