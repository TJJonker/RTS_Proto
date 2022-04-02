using Jonko.Utils;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement
{
    // Formations
    public IFormation circleFormation = new CircleFormation();

    private IFormation currentFormation;


    public void MoveUnits(List<RTSUnit> selectedUnits)
    {
        var targetPosition = Utils.MouseToScreen(Input.mousePosition);
        MoveTroops(currentFormation.GetPositionList(targetPosition), selectedUnits);
    }

    private void MoveTroops(List<Vector3> targetPositionList, List<RTSUnit> selectedUnits)
    {
        int targetPositionListIndex = 0;

        foreach (RTSUnit unit in selectedUnits)
        {
            unit.MoveTo(targetPositionList[targetPositionListIndex]);
            targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
        }
    }

    public void SwitchFormation(IFormation formation) => currentFormation = formation;
}
