using UnityEngine;
using UnityEngine.InputSystem;
using Jonko.Utils;
using RTS.Input;
using Jonko.Grids;

public class GridTesting : MonoBehaviour
{
    [SerializeField] private HeatMapVisual heatMapVisual;

    private PlayerInputActionMaps playerInputActionMaps;

    private Grid<bool> grid;


    private void Start()
    {
        playerInputActionMaps = InputManager.Instance.PlayerInputActionMap;

        playerInputActionMaps.Gameplay.LeftMouse.started += ChangeValue;

        grid = new Grid<bool>(20, 20, .75f, new Vector2(-5, -3));
        //heatMapVisual.SetGrid(grid);
    }

    private void ChangeValue(InputAction.CallbackContext context)
    {
        Vector3 mouseWorldPosition = Utils.MouseToScreen(Mouse.current.position.ReadValue());
        //grid.AddRangedValue(mouseWorldPosition, 100, 3, 20);
    }
}
