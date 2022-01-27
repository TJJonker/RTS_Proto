using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject selectionSquare;

    private Vector2 selectionStart;

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
        }

        // Release left mouse button
        if (Input.GetMouseButtonUp(0))
        {
            selectionSquare.SetActive(false);
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