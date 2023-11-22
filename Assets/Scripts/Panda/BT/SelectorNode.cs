using System.Collections.Generic;


/// <summary> 자식 노드에서 처음으로 Success 나 Running 상태를 가진 노드가 발생하면 그 노드까지 진행하고 멈추는 노드 </summary>
public class SelectorNode : INode
{
    private List<INode> _childs;

    public SelectorNode(List<INode> childs)
    {
        _childs = childs;
    }

    public INode.ENodeState Evaluate()
    {
        if (_childs == null)
            return INode.ENodeState.Failure;


        foreach (INode child in _childs)
        {
            switch (child.Evaluate())
            {
                //자식 상태: Running일 때 -> Running 반환
                case INode.ENodeState.Running:
                    return INode.ENodeState.Running;

                //자식 상태: Success 일 때->Success 반환
                case INode.ENodeState.Success:
                    return INode.ENodeState.Success;

                    //자식 상태: Failure일 때 -> 다음 자식으로 이동
            }
        }

        return INode.ENodeState.Failure;
    }
}
