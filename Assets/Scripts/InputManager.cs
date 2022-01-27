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
            DeselectUnits();
            SelectUnits();
        }

        // Release left mouse button
        if (Input.GetMouseButtonUp(0))
        {
            selectionSquare.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            var targetPosition = MouseToScreen(Input.mousePosition);
            List<Vector3> targetPositionList = GetPositionListAround(targetPosition, new float[] {.5f, 1f, 1.5f}, new int[] {6, 12, 24});

            int targetPositionListIndex = 0;

            foreach(UnitRS unit in selectedUnits)
            {
                unit.GetComponent<Movement>().MoveObject(targetPositionList[targetPositionListIndex]);
                targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
            }
        }
    }

    private void DeselectUnits()
    {
        foreach (UnitRS unit in selectedUnits)
            unit.SetSelectedActive(false);
        selectedUnits.Clear();
    }

    private void SelectUnits()
    {
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

    /// <summary>
    ///     Creates an amount of positions in the shape of a circle
    /// </summary>
    /// <param name="startPosition"> Center position </param>
    /// <param name="distance"> Distance form inner point of the circle to the positions </param>
    /// <param name="positionCount"> Amount of positions </param>
    /// <returns></returns>
    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
    {
        List<Vector3> positionList = new List<Vector3>();
        for(int i = 0; i < positionCount; i++)
        {
            float angle = i * (360 / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = startPosition + dir * distance;
            positionList.Add(position);
        }
        return positionList;
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();   
        positionList.Add(startPosition);
        for(int i = 0; i <ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionList;
    }

    private Vector3 ApplyRotationToVector(Vector3 vec, float angle) 
        => Quaternion.Euler(0, 0, angle) * vec;
}