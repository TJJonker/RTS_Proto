using Jonko.Utils;
using UnityEngine;
using Jonko.Grids;

public class HeatMapBoolVisual : MonoBehaviour
{
    private Grid<bool> grid;
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

    private void OnGridValueChanged(object sender, Grid<bool>.EAOnGridValueChanged e)
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

    public void SetGrid(Grid<bool> grid) => this.grid = grid;

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

                bool gridValue = grid.GetGridObject(x, y);
                float gridValueNormalized = gridValue ? 1f : 0f;
                Vector2 gridValueUV = new Vector2(gridValueNormalized, 0f);

                MeshUtils.AddToMeshArray(ref vertices, ref uv, ref triangles, index, grid.GetWorldPosition(x, y) + quadSize / 2f, 0f, quadSize, gridValueUV, gridValueUV);
            }
        }
        MeshUtils.ApplyToMesh(mesh, vertices, uv, triangles);
    }
}
