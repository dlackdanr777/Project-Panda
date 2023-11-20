
public interface INode
{
    public enum ENodeState {Running, Success, Failure}

    /// <summary> 현재 노드가 어떤 상태인지 반환 </summary>
    public ENodeState Evaluate();
}
