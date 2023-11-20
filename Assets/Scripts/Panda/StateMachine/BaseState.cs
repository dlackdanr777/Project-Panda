
public abstract class BaseState
{

    /// <summary> 다른상태에서 이 상태로 변화될때 한번 실행  </summary>
    public abstract void OnEnter();

    /// <summary> 상태가 변화되고 Update와 연동되어 지속 실행  </summary>
    public abstract void OnUpdate();

    /// <summary> 상태가 변화되고 FixedUpdate와 연동되어 지속 실행  </summary>
    public abstract void OnFixedUpdate();

    /// <summary> 이 상태에서 다른 상태로 변화할때 한번 실행  </summary>
    public abstract void OnExit();

}
