using UnityEngine;

namespace Jonko.Utils
{
    public class MeshUtils
    {

        /// <summary>
        ///     Creates an empty mesh 
        /// </summary>
        /// <returns> Returns an empty mesh </returns>
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

        /// <summary>
        ///     Adds a single quad directly to the referenced mesh
        /// </summary>
        /// <param name="mesh"> Mesh to add a quad to </param>
        /// <param name="pos"> World space position of the quad </param>
        /// <param name="rot"> Rotation of the quad </param>
        /// <param name="baseSize"> Size of the quad </param>
        /// <param name="uv00"> First UV position of the material </param>
        /// <param name="uv11"> Second UV position of the material </param>
        public static void AddToMesh(ref Mesh mesh, Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
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
        }

        /// <summary>
        ///     Adds a single quad to the given mesh arrays at the given index
        /// </summary>
        /// <param name="vertices"> Array of vertices to add the quad to </param>
        /// <param name="uvs"> Array of uvs to add the quad to </param>
        /// <param name="triangles"> Array of triangles to add the quad to </param>
        /// <param name="index"> Index of the quad in the arrays </param>
        /// <param name="pos"> World space position of the quad </param>
        /// <param name="rotation"> Rotation of the quad </param>
        /// <param name="baseSize"> Size of the quad </param>
        /// <param name="uv00"> First UV position of the material </param>
        /// <param name="uv11"> Second UV position of the material </param>
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

        /// <summary>
        ///     Applies the vertices, uvs and triangles to the mesh object
        /// </summary>
        /// <param name="mesh"> Mesh to apply the params to </param>
        /// <param name="vertices"> Vertice array to apply </param>
        /// <param name="uvs"> UV array to apply </param>
        /// <param name="triangles"> triangle array to apply </param>
        public static void ApplyToMesh(Mesh mesh, Vector3[] vertices, Vector2[] uvs, int[] triangles)
        {
            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;
        }
        
        #region Private/Helper Functions
        /// <summary>
        ///     Converts a float rotation to an Euler rotation on the Z-Axis
        /// </summary>
        /// <param name="rotation"> Float rotation </param>
        /// <returns> Euler rotation on the Z-Axis </returns>
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