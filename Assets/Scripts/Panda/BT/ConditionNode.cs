using System;
using System.Diagnostics;
using Unity.VisualScripting;

/// <summary> 조건문을 확인 후 true일 경우 하위 Node실행 false일경우 Failure을 반환하는 노드</summary>
public class ConditionNode : INode
{
    private INode _child;
    private Func<bool> _condition;

    public ConditionNode(INode child, Func<bool> condition)
    {
        _child = child;
        _condition = condition;
    }

    public INode.ENodeState Evaluate()
    {
        if(_condition == null)
            return INode.ENodeState.Failure;

        if (_condition())
        {
            return _child.Evaluate();
        }

        return INode.ENodeState.Failure;
    }
}
