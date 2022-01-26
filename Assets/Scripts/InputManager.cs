using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Remove
    [SerializeField] private Movement move;
    [SerializeField] private Camera mainCamera;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) move.MoveObject(MouseToScreen(Input.mousePosition));    
    }

    private Vector3 MouseToScreen(Vector3 mousePosition)
    {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }
}