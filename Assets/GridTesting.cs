using UnityEngine;
using UnityEngine.InputSystem;
using Grid = Jonko.Grids.Grid;
using Jonko.Utils;
using RTS.Input;

public class GridTesting : MonoBehaviour
{
    [SerializeField] private HeatMapVisual heatMapVisual;

    private PlayerInputActionMaps playerInputActionMaps;

    private Grid grid;


    private void Start()
    {
        playerInputActionMaps = InputManager.Instance.PlayerInputActionMap;

        playerInputActionMaps.Gameplay.LeftMouse.started += ChangeValue;

        grid = new Grid(5, 10, .75f, new Vector2(-5, -3));
        heatMapVisual.SetGrid(grid);
    }

    private void ChangeValue(InputAction.CallbackContext context)
    {
        Vector3 mouseWorldPosition = Utils.MouseToScreen(Mouse.current.position.ReadValue());
        int value = grid.GetValue(mouseWorldPosition);
        grid.SetValue(mouseWorldPosition, value + 5);
    }
}
