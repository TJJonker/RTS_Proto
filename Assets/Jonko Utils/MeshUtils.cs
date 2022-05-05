using UnityEngine;

namespace Jonko.Utils
{
    public class MeshUtils
    {
        public static Mesh CreateEmptyMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = new Vector3[0];
            mesh.uv = new Vector2[0];
            mesh.triangles = new int[0];
            return mesh;
        }

        /// <summary>
        ///     Creates the necessary array(sizes) to create a mesh with a given amount of quads
        /// </summary>
        /// <param name="quadCount"> Amount of desired quads </param>
        /// <param name="vertices"> Array of vertices to create </param>
        /// <param name="uvs"> Array of uvs to create </param>
        /// <param name="triangles"> Array of triangles to create </param>
        public static void CreateEmptyMeshArrays(int quadCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles)
        {
            vertices = new Vector3[4 * quadCount];
            uvs = new Vector2[4 * quadCount];
            triangles = new int[6 * quadCount];
        }

        public static Mesh AddToMesh(ref Mesh mesh, Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
        {
            // Create if mesh does not exist
            if(mesh == null) mesh = CreateEmptyMesh();

            // Create new arrays
            Vector3[] vertices = new Vector3[mesh.vertices.Length + 4];
            Vector2[] uvs = new Vector2[(mesh.uv.Length + 4)];
            int[] triangles = new int[mesh.triangles.Length + 6];

            // Copy existing information to the arrays
            mesh.vertices.CopyTo(vertices, 0);
            mesh.uv.CopyTo(uvs, 0);
            mesh.triangles.CopyTo(triangles, 0);

            // Go to the end of the array, to the empty spaces
            // EX: 16 / 4 - 1 = 3. The vIndex will add vertices fromout that index to access the last 4 indexes.
            int index = vertices.Length / 4 - 1;

            AddToMeshArray(ref vertices, ref uvs, ref triangles, index, pos, rot, baseSize, uv00, uv11);

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;

            return mesh;
        }

        public static void AddToMeshArray(ref Vector3[] vertices, ref Vector2[] uvs, ref int[] triangles, int index, Vector3 pos, 
            float rotation, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
        {
            // Store the 4 indexes for easy access
            int vIndex = index * 4;
            int vIndex0 = vIndex;
            int vIndex1 = vIndex + 1;
            int vIndex2 = vIndex + 2;
            int vIndex3 = vIndex + 3;

            // Determine the center of the mesh
            baseSize *= .5f;

            // Check whether the object is a square
            if (baseSize.x != baseSize.y)
            {
                // Position the vertices in the game world space
                vertices[vIndex0] = pos + GetQuaternionEuler(rotation) * new Vector3(-baseSize.x, baseSize.y);
                vertices[vIndex1] = pos + GetQuaternionEuler(rotation) * -baseSize;
                vertices[vIndex2] = pos + GetQuaternionEuler(rotation) * new Vector3(baseSize.x, -baseSize.y);
                vertices[vIndex3] = pos + GetQuaternionEuler(rotation) * baseSize;
            }
            else
            {
                vertices[vIndex0] = pos + GetQuaternionEuler(rotation - 270) * baseSize;
                vertices[vIndex1] = pos + GetQuaternionEuler(rotation - 180) * baseSize;
                vertices[vIndex2] = pos + GetQuaternionEuler(rotation - 90) * baseSize;
                vertices[vIndex3] = pos + GetQuaternionEuler(rotation) * baseSize;
            }

            // Rellocate uvs
            uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
            uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
            uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
            uvs[vIndex3] = new Vector2(uv11.x, uv11.y);

            // Create triangles
            int tIndex = index * 6;

            triangles[tIndex] = vIndex0;
            triangles[tIndex + 1] = vIndex3;
            triangles[tIndex + 2] = vIndex1;

            triangles[tIndex + 3] = vIndex1;
            triangles[tIndex + 4] = vIndex3;
            triangles[tIndex + 5] = vIndex2;
        }


        #region Private/Helper Functions
        private static Quaternion GetQuaternionEuler(float rotation)
        {
            int rot = Mathf.RoundToInt(rotation);
            rot = rot % 360;
            if (rot < 0) rot += 360;
            return Quaternion.Euler(0, 0, rot);
        }
        #endregion
    }
}