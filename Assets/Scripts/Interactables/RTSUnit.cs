using System;
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

<<<<<<< Updated upstream
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
=======
    /// <summary>
    ///     Enables or disables the selected visual
    /// </summary>
    /// <param name="active"> Whether or not the selection should show </param>
    public void SetSelectedActive(bool active) => selectedSpriteObject.SetActive(active);

    /// <summary>
    ///     Moves the unit to a certain position
    /// </summary>
    /// <param name="desiredPosition"> the desired position to move the unit to </param>
    public void MoveTo(Vector2 desiredPosition, Action action = null) => movement.MoveTo(desiredPosition, action);
>>>>>>> Stashed changes
}
