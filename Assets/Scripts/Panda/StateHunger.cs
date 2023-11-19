using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHunger : IState
{
    public StateHunger() { }
    public void OperateEnter()
    {
        // 배고픔 이모티콘 띄우기 추가
        Debug.Log("배고픔");
    }
    public void OperateUpdate()
    {

    }
    public void OperateExit()
    {

    }
}
