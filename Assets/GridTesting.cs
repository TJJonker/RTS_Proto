using UnityEngine;
using UnityEngine.InputSystem;
using Jonko.Utils;
using RTS.Input;
using Jonko.Grids;

public class GridTesting : MonoBehaviour
{
    [SerializeField] private HeatMapBoolVisual heatMapBoolVisual;

    private PlayerInputActionMaps playerInputActionMaps;

    private Grid<HeatMapGridObject> grid;


    private void Awake()
    {
        playerInputActionMaps = InputManager.Instance.PlayerInputActionMap;

        playerInputActionMaps.Gameplay.LeftMouse.started += ChangeValue;

        grid = new Grid<HeatMapGridObject>(20, 20, .75f, new Vector2(-5, -3), () => new HeatMapGridObject());
        //heatMapBoolVisual.SetGrid(grid);
    }

    private void ChangeValue(InputAction.CallbackContext context)
    {
        Vector3 mouseWorldPosition = Utils.MouseToScreen(Mouse.current.position.ReadValue());
        HeatMapGridObject heatMapGridObject = grid.GetGridObject(mouseWorldPosition);
    }
}

public class HeatMapGridObject
{
    private const int MIN = 0;
    private const int MAX = 100;

    public int value;

    public void AddValue(int addValue)
    {
        value += addValue;
        value = Mathf.Clamp(value, MIN, MAX);
    }

    public float GetValueNormalized()
        => (float)value / MAX;
}
