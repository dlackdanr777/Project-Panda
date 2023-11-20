
public abstract class BaseState
{

    /// <summary> �ٸ����¿��� �� ���·� ��ȭ�ɶ� �ѹ� ����  </summary>
    public abstract void OnEnter();

    /// <summary> ���°� ��ȭ�ǰ� Update�� �����Ǿ� ���� ����  </summary>
    public abstract void OnUpdate();

    /// <summary> ���°� ��ȭ�ǰ� FixedUpdate�� �����Ǿ� ���� ����  </summary>
    public abstract void OnFixedUpdate();

    /// <summary> �� ���¿��� �ٸ� ���·� ��ȭ�Ҷ� �ѹ� ����  </summary>
    public abstract void OnExit();

}
