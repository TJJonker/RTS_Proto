public interface IUnitState
{
    public void EnterState(RTSUnit unit);

    public void UpdateState();

    public void ExitState();

}
