using UnityEngine;
using Grid = Jonko.Grids.Grid;

public class GridTesting : MonoBehaviour
{
    private void Start()
    {
        Grid grid = new Grid(20, 10);
    }
}
