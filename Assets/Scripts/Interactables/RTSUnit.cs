using UnityEngine;

[RequireComponent(typeof(MovementState))]
public class RTSUnit : MonoBehaviour
{
    private GameObject selectedGameObject;

    private IUnitState UnitStateMoving;
    private IUnitState UnitStateGathering = new GatheringState();

    private IUnitState currentState;

    private void Awake()
    {
        // Retreving important objects
        selectedGameObject = transform.Find("Selection").gameObject;
        UnitStateMoving = GetComponent<MovementState>();

        // Determining first state
        SwitchState(UnitStateMoving);
    }

    private void Update() => currentState.UpdateState();

    public void SwitchState(IUnitState state)
    {
        if(currentState != null) currentState.ExitState();
        currentState = state;
        currentState.EnterState(this);
    }

    public void SetSelectedActive(bool active) => selectedGameObject.SetActive(active);

    public void RequestMovement(Vector2 desiredPosition)
    {
        if(currentState != UnitStateMoving) SwitchState(UnitStateMoving);
        GetComponent<MovementState>().MoveUnit(desiredPosition);
    }
}
