using System;
using UnityEngine;

public class RTSUnit : MonoBehaviour
{
    private GameObject selectedSpriteObject;
    private RTSMovement movement;

    private void Awake()
    {
        // Retreving important objects
        selectedSpriteObject = transform.Find("selection").gameObject;

        movement = GetComponent<RTSMovement>(); 
    }

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

    public void VictoryDance(Action action = null) => movement.Rotate(action);
}