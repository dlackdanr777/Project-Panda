
/// <summary> 상호 작용 인터페이스 </summary>
public interface IInteraction
{
    /// <summary> 상호작용을 시작할때 실행될 함수 </summary>
    public void StartInteraction();

    /// <summary> 상호작용중에 Update와 연동하여 실행될 함수 </summary>
    public void UpdateInteraction();

    /// <summary> 상호작용을 종료할때 실행될 함수 </summary>
    public void ExitInteraction();
}
