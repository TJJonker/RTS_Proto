using UnityEngine;
using Grid = Jonko.Grids.Grid;

public class HeatMapVisual : MonoBehaviour
{
    private Grid grid;

    public void SetGrid(Grid grid) => this.grid = grid;
}
