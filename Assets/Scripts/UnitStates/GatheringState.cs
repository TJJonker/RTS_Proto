public class GatheringState : IUnitState
{

    public void EnterState(RTSUnit unit)
    {
        // Enabling pickaxe visual
    }

    public void UpdateState()
    {
        Mine();
    }

    public void ExitState()
    {
    }

    private void Mine()
    {
        // Start animating unit
    }

    public void Collect()
    {
        
    }



}
