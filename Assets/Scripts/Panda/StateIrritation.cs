using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIrritation : IState
{
    public void OperateEnter()
    {
        // 짜증 이모티콘 띄우기 추가
        Debug.Log("짜증");
    }
    public void OperateUpdate()
    {

    }
    public void OperateExit()
    {

    }
}
