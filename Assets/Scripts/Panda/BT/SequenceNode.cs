using System.Collections.Generic;


/// <summary> 자식 노드를 순서대로 진행하면서 Failure 상태가 나올 때까지 진행하는 노드 </summary>
public class SequenceNode : INode
{
    private List<INode> _childs;

    public SequenceNode(List<INode> childs)
    {
        _childs = childs;
    }

    public INode.ENodeState Evaluate()
    {

        if (_childs == null || _childs.Count == 0)
            return INode.ENodeState.Failure;

        foreach (INode child in _childs)
        {
            switch (child.Evaluate())
            {
                case INode.ENodeState.Running:
                    return INode.ENodeState.Running;

                //자식 상태: Success 일 때->다음 자식으로 이동
                case INode.ENodeState.Success:
                    continue;

                case INode.ENodeState.Failure:
                    return INode.ENodeState.Failure;
            }
        }

        return INode.ENodeState.Failure;
    }

}
