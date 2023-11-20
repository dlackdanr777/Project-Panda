using System;


/// <summary> 실제로 어떤 행위를 하는 노드 </summary>
public class ActionNode : INode
{
    private Func<INode.ENodeState> _onUpdate;

    public ActionNode(Func<INode.ENodeState> onUpdate)
    {
        _onUpdate = onUpdate;
    }

    public INode.ENodeState Evaluate() => _onUpdate?.Invoke() ?? INode.ENodeState.Failure;

}
