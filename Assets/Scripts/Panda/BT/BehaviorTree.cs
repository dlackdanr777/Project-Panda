
/// <summary> BehaviorTree를 관리하는 클래스</summary>
public class BehaviorTree
{
    private INode _rootNode;

    public BehaviorTree(INode rootNode)
    {
        _rootNode = rootNode;
    }

    public void Operate() => _rootNode.Evaluate();
}
