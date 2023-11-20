
public abstract class StarterPandaState : BaseState
{
    protected StarterPanda _panda;

    protected StarterPandaStateMachine _machine;

    public StarterPandaState(StarterPanda panda, StarterPandaStateMachine machine)
    {
        _panda = panda;
        _machine = machine;
    }
}
