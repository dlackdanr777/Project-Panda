
public abstract class StateMachine
{
    /// <summary> 상태가 변화되고 Update와 연동되어 지속 실행  </summary>
    public abstract void OnStateUpdate();

    /// <summary> 상태가 변화되고 FixedUpdate와 연동되어 지속 실행  </summary>
    public abstract void OnStateFixedUpdate();

}
