
/// <summary> ��ȣ �ۿ� �������̽� </summary>
public interface IInteraction
{
    /// <summary> ��ȣ�ۿ��� �����Ҷ� ����� �Լ� </summary>
    public void StartInteraction();

    /// <summary> ��ȣ�ۿ��߿� Update�� �����Ͽ� ����� �Լ� </summary>
    public void UpdateInteraction();

    /// <summary> ��ȣ�ۿ��� �����Ҷ� ����� �Լ� </summary>
    public void ExitInteraction();
}
