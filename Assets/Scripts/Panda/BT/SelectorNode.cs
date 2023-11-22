using System.Collections.Generic;


/// <summary> �ڽ� ��忡�� ó������ Success �� Running ���¸� ���� ��尡 �߻��ϸ� �� ������ �����ϰ� ���ߴ� ��� </summary>
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

<<<<<<< HEAD
=======

>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")
        foreach (INode child in _childs)
        {
            switch(child.Evaluate())
            {
<<<<<<< HEAD
                case INode.ENodeState.Running:
                    return INode.ENodeState.Running;

                case INode.ENodeState.Success:
                    return INode.ENodeState.Success;
=======
                //�ڽ� ����: Running�� �� -> Running ��ȯ
                case INode.ENodeState.Running:
                    return INode.ENodeState.Running;

                //�ڽ� ����: Success �� ��->Success ��ȯ
                case INode.ENodeState.Success:
                    return INode.ENodeState.Success;

                //�ڽ� ����: Failure�� �� -> ���� �ڽ����� �̵�
>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")
            }
        }

        return INode.ENodeState.Failure;
    }
}
