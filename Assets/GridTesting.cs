using UnityEngine;
using UnityEngine.InputSystem;
using Jonko.Utils;
using RTS.Input;
using Jonko.Grids;

public class GridTesting : MonoBehaviour
{
    private PlayerInputActionMaps playerInputActionMaps;

    private Grid<HeatMapGridObject> grid;


    private void Awake()
    {
        playerInputActionMaps = InputManager.Instance.PlayerInputActionMap;

        playerInputActionMaps.Gameplay.LeftMouse.started += ChangeValue;

        grid = new Grid<HeatMapGridObject>(20, 20, .75f, new Vector2(-5, -3), (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));
    }

    private void ChangeValue(InputAction.CallbackContext context)
    {
        Vector3 mouseWorldPosition = Utils.MouseToScreen(Mouse.current.position.ReadValue());
        HeatMapGridObject heatMapGridObject = grid.GetGridObject(mouseWorldPosition);
        if (heatMapGridObject != null)
            heatMapGridObject.AddValue(5);
    }
}

public class HeatMapGridObject
{
    private const int MIN = 0;
    private const int MAX = 100;

    private Grid<HeatMapGridObject> grid;
    private int x, y;
    private int value;

    public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void AddValue(int addValue)
    {
        value += addValue;
        value = Mathf.Clamp(value, MIN, MAX);
        grid.TriggerGridObjectChanged(x, y);
    }

    public float GetValueNormalized()
        => (float)value / MAX;

    public override string ToString()
        => value.ToString();
}
