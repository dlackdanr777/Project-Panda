using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDepression : IState
{
    public void OperateEnter()
    {
        // ��� �̸�Ƽ�� ���� �߰�
        Debug.Log("���");
    }
    public void OperateUpdate()
    {

    }
    public void OperateExit()
    {

    }
}
