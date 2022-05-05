using Jonko.Utils;
using UnityEngine;
using Grid = Jonko.Grids.Grid;

public class HeatMapVisual : MonoBehaviour
{
    private Grid grid;
    private Mesh mesh;
    private MeshFilter meshFilter;

    private void Awake()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    private void Start()
    {
        UpdateHeatMapsVisual();

        grid.OnGridValueChanged += OnGridValueChanged;
    }

    private void OnGridValueChanged(object sender, Grid.EAOnGridValueChanged e)
    {
        UpdateHeatMapsVisual();
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
                Debug.Log(gridValue);
                float gridValueNormalized = (float)gridValue / Grid.HEAT_MAP_MAX_VALUE;
                Vector2 gridValueUV = new Vector2(gridValueNormalized, 0f);

                MeshUtils.AddToMeshArray(ref vertices, ref uv, ref triangles, index, grid.GetWorldPosition(x, y) + quadSize / 2f, 0f, quadSize, gridValueUV, gridValueUV);
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
