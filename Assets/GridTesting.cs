using UnityEngine;
using UnityEngine.InputSystem;
using Grid = Jonko.Grids.Grid;
using Jonko.Utils;

public class GridTesting : MonoBehaviour
{
    private Grid grid;

    private void Start()
    {
        grid = new Grid(20, 10, .5f, new Vector2(-5, -3));
    }

    private void Update()
    {
        if (Mouse.current.leftButton.IsPressed())
        {
            grid.SetValue(Utils.MouseToScreen(Mouse.current.position.ReadValue()), 69);
        }
    }
}
