using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHappiness : IState
{
    public void OperateEnter()
    {
        // 행복 이모티콘 띄우기 추가
        Debug.Log("행복");
    }
    public void OperateUpdate()
    {

    }
    public void OperateExit()
    {

    }
}
