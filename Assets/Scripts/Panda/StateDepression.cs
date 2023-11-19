using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDepression : IState
{
    public void OperateEnter()
    {
        // 우울 이모티콘 띄우기 추가
        Debug.Log("우울");
    }
    public void OperateUpdate()
    {

    }
    public void OperateExit()
    {

    }
}
