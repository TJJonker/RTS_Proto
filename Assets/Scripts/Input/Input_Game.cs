using Jonko.Patterns;
using System.Collections.Generic;
using UnityEngine;

public class Input_Game : IStatePattern
{
    private Vector2 selectionStart;

    private List<RTSUnit> selectedUnits = new List<RTSUnit>();

    // Actions
    private UnitSelection unitSelection = new UnitSelection();
    private UnitMovement unitMovement = new UnitMovement();

    private GameObject selectionSquare;

    public void EnterState()
    {
        unitMovement.SwitchFormation(unitMovement.circleFormation);
        //this.selectionSquare = InputManager.Instance.selectionSquare;
    }

    public void ExitState()
    {
       
    }

    public void UpdateState()
    {
        /*
        // Click left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            selectionStart = Utils.MouseToScreen(Input.mousePosition);
            selectionSquare.SetActive(true);
        }

        // Hold left mouse button
        if (Input.GetMouseButton(0))
        {
            unitSelection.DrawSelectionSquare(selectionSquare, selectionStart, Utils.MouseToScreen(Input.mousePosition));
            unitSelection.DeselectUnits(selectedUnits);
            unitSelection.SelectUnits(selectedUnits, selectionStart, Utils.MouseToScreen(Input.mousePosition));
        }

        // Release left mouse button
        if (Input.GetMouseButtonUp(0))
        {
            selectionSquare.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            unitMovement.MoveUnits(selectedUnits);
        }
        */
    }
}
