using Game.PathFinding;
using Jonko.Grids;
using Jonko.Utils;
using UnityEngine;

public class PathFindingVisual : MonoBehaviour
{
    private Grid<PathNode> grid;
    private Mesh mesh;
    private bool updateMesh;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh; 
    }

    public void SetGrid(Grid<PathNode> grid)
    {
        this.grid = grid;
        UpdateVisual();
        grid.OnGridValueChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, Grid<PathNode>.EAOnGridValueChanged e)
        => updateMesh = true;

    private void LateUpdate()
    {
        if (!updateMesh) return;
        updateMesh = false; 
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices,
            out Vector2[] uvs, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for(int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = Vector3.one * grid.GetCellSize();

                PathNode gridObject = grid.GetGridObject(x, y);
                float gridValueNormalized = gridObject.isWalkable ? 0 : 1;
                Vector2 gridValueUV = new Vector2(gridValueNormalized, 0);

                MeshUtils.AddToMeshArray(ref vertices, ref uvs, ref triangles, 
                    index, grid.GetWorldPosition(x, y) + quadSize / 2f, 0f, quadSize, gridValueUV, gridValueUV);
            }
        }
        MeshUtils.ApplyToMesh(mesh, vertices, uvs, triangles);
    }
}
