using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject selectionSquare;

    private Vector2 selectionStart;

    private List<UnitRS> selectedUnits = new List<UnitRS>();

    private void Update()
    {
        // Click left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            selectionStart = MouseToScreen(Input.mousePosition);
            selectionSquare.SetActive(true);
        }

        // Hold left mouse button
        if (Input.GetMouseButton(0))
        {
            DrawSelectionSquare();
            SelectUnits();

        }

        // Release left mouse button
        if (Input.GetMouseButtonUp(0))
        {

            selectionSquare.SetActive(false);
        }
    }

    private void SelectUnits()
    {
        foreach (UnitRS unit in selectedUnits)
            unit.SetSelectedActive(false);
        selectedUnits.Clear();

        Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(selectionStart, MouseToScreen(Input.mousePosition));
        foreach (Collider2D collider in collider2DArray)
        {
            UnitRS unitRS = collider.GetComponent<UnitRS>();
            if (unitRS != null)
            {
                selectedUnits.Add(unitRS);
                unitRS.SetSelectedActive(true);
            }
        }
    }

    private void DrawSelectionSquare()
    {
        var currentMousePos = MouseToScreen(Input.mousePosition);
        var lowerLeft = new Vector2(
            Mathf.Min(selectionStart.x, currentMousePos.x),
            Mathf.Min(selectionStart.y, currentMousePos.y)
            );
        var upperRight = new Vector2(
            Mathf.Max(selectionStart.x, currentMousePos.x),
            Mathf.Max(selectionStart.y, currentMousePos.y)
        );
        selectionSquare.transform.position = lowerLeft;
        selectionSquare.transform.localScale = upperRight - lowerLeft;
    }

    private Vector3 MouseToScreen(Vector3 mousePosition)
    {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }
}