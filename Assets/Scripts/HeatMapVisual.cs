using Jonko.Utils;
using UnityEngine;
using Grid = Jonko.Grids.Grid;

public class HeatMapVisual : MonoBehaviour
{
    private Grid grid;
    private Mesh mesh;
    private bool updateMesh;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Start()
    {
        UpdateHeatMapsVisual();

        grid.OnGridValueChanged += OnGridValueChanged;
    }

    private void OnGridValueChanged(object sender, Grid.EAOnGridValueChanged e)
    {
        updateMesh = true;
    }

    private void LateUpdate()
    {
        if (updateMesh)
        {
            updateMesh = false;
            UpdateHeatMapsVisual();
        }
    }

    public void SetGrid(Grid grid) => this.grid = grid;

    private void UpdateHeatMapsVisual()
    {
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, 
            out Vector2[] uv, out int[] triangles);

        for(int x = 0; x < grid.GetWidth(); x++)
        {
            for(int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = Vector3.one * grid.GetCellSize();

                int gridValue = grid.GetValue(x, y);
                float gridValueNormalized = (float)gridValue / Grid.HEAT_MAP_MAX_VALUE;
                Vector2 gridValueUV = new Vector2(gridValueNormalized, 0f);

                MeshUtils.AddToMeshArray(ref vertices, ref uv, ref triangles, index, grid.GetWorldPosition(x, y) + quadSize / 2f, 0f, quadSize, gridValueUV, gridValueUV);
            }
        }
        MeshUtils.ApplyToMesh(mesh, vertices, uv, triangles);
    }
}
