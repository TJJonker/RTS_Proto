using System.Collections.Generic;
using UnityEngine;

public class UnitSelection
{
    public void DeselectUnits(List<RTSUnit> selectedUnits)
    {
        foreach (RTSUnit unit in selectedUnits)
            unit.SetSelectedActive(false);
        selectedUnits.Clear();
    }

    public void SelectUnits(List<RTSUnit> selectedUnits, Vector2 selectionStart, Vector2 selectionEnd)
    {
        Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(selectionStart, selectionEnd);
        foreach (Collider2D collider in collider2DArray)
        {
            RTSUnit unitRS = collider.GetComponent<RTSUnit>();
            if (unitRS != null)
            {
                selectedUnits.Add(unitRS);
                unitRS.SetSelectedActive(true);
            }
        }
    }

    public void DrawSelectionSquare(GameObject selectionSquare, Vector2 selectionStart, Vector2 selectionEnd)
    {
        var lowerLeft = new Vector2(
            Mathf.Min(selectionStart.x, selectionEnd.x),
            Mathf.Min(selectionStart.y, selectionEnd.y)
            );
        var upperRight = new Vector2(
            Mathf.Max(selectionStart.x, selectionEnd.x),
            Mathf.Max(selectionStart.y, selectionEnd.y)
        );
        selectionSquare.transform.position = lowerLeft;
        selectionSquare.transform.localScale = upperRight - lowerLeft;
    }
}
