using UnityEngine;

namespace Jonko.Visualisation
{
    public class CustomMesh
    {
        private GameObject gameObject;

        /// <summary>
        ///     Creates a custom mesh with a modulair anount of quads (two triangles)
        /// </summary>
        /// <param name="position"> Position of the mesh </param>
        /// <param name="scale"> Scale of the mesh </param>
        /// <param name="ZRotation"> Rotation on the Z-Axis </param>
        /// <param name="material"> Material used on the mesh </param>
        /// <param name="vertices"> array of vertices </param>
        /// <param name="uv"> array of UVs </param>
        /// <param name="triangles"> array of triangles </param>
        /// <param name="parent"> Parent object of the mesh </param>
        /// <param name="sortingOrder"> Sorting order of the mesh </param>
        /// <param name="objectName"> Name of the newly made GameObject </param>
        /// <returns> Returns the created CustomMesh instance </returns>
        public static CustomMesh Create(Vector3 position, Vector3 scale, float ZRotation, Material material, Vector3[] vertices, 
            Vector2[] uv, int[] triangles, Transform parent = null, int sortingOrder = 0, string objectName = "Mesh")
            => new CustomMesh(parent, objectName, position, scale, ZRotation, material, vertices, uv, triangles, sortingOrder);

        /// <summary>
        ///     Creates a custom mesh with a single quad (two triangles)
        /// </summary>
        /// <param name="position"> Position of the mesh </param>
        /// <param name="scale"> Scale of the mesh </param>
        /// <param name="ZRotation"> Rotation on the Z-Axis </param>
        /// <param name="material"> Material used on the mesh </param>
        /// <param name="meshWidth"> Width of the mesh </param>
        /// <param name="meshHeight"> Height of the mesh </param>
        /// <param name="parent"> Parent object of the mesh </param>
        /// <param name="uvCoords"> UV Coords in case of the usage of a SpriteSheet </param>
        /// <param name="sortingOrder"> Sorting order of the mesh </param>
        /// <param name="objectName"> Name of the newly made GameObject </param>
        /// <returns> Returns the created CustomMesh instance </returns>
        public static CustomMesh Create(Vector3 position, Vector3 scale, float ZRotation, Material material, float meshWidth,
            float meshHeight, Transform parent, UVCoords uvCoords = null, int sortingOrder = 0, string objectName = "Mesh")
            => new CustomMesh(parent, objectName, position, scale, ZRotation, meshWidth, meshHeight, material, uvCoords, sortingOrder);


        #region Constructors
        /// <summary>
        ///     Creates a custom mesh with a single quad (two triangles)
        /// </summary>
        /// <param name="parent"> Parent object of the mesh </param>
        /// <param name="ObjectName"> Name of the newly made GameObject </param>
        /// <param name="localPosition"> Local position of the mesh </param>
        /// <param name="localScale"> Local scale of the mesh </param>
        /// <param name="ZRotation"> Rotation on the Z-Axis </param>
        /// <param name="meshWidth"> Width of the mesh </param>
        /// <param name="meshHeight"> Height of the mesh </param>
        /// <param name="material"> Material used on the mesh </param>
        /// <param name="uvCoords"> UV Coords in case of the usage of a SpriteSheet </param>
        /// <param name="sortingOrder"> Sorting order of the mesh </param>
        public CustomMesh(Transform parent, string ObjectName, Vector3 localPosition, Vector3 localScale, float ZRotation, 
            float meshWidth, float meshHeight, Material material, UVCoords uvCoords, int sortingOrder)
        {
            // Creating new necessary mesh arrays
            var vertices = new Vector3[4];
            var uv = new Vector2[4];
            var triangles = new int[6];

            float meshWidthHalf = meshWidth / 2f;
            float meshHeightHalf = meshHeight / 2f;

            // Positioning the vertices
            vertices[0] = new Vector3(-meshWidthHalf, meshHeightHalf);
            vertices[1] = new Vector3(meshWidthHalf, meshHeightHalf);
            vertices[2] = new Vector3(-meshWidthHalf, -meshHeightHalf);
            vertices[3] = new Vector3(meshWidthHalf, -meshHeightHalf);

            if(uvCoords == null) 
                uvCoords = new UVCoords(0, 0, material.mainTexture.width, material.mainTexture.height);

            Vector2[] uvArray = GetUVRectangleFromPixels(uvCoords, material);

            ApplyUVToUVArray(uvArray, ref uv);

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 2;
            triangles[4] = 1;
            triangles[5] = 3;

            new CustomMesh(parent, ObjectName, localPosition, localScale, ZRotation, 
                material, vertices, uv, triangles, sortingOrder);
        }

        /// <summary>
        ///     Creates a custom mesh with a modulair amount of quads (two triangles)
        /// </summary>
        /// <param name="parent"> Parent object of the mesh </param>
        /// <param name="ObjectName"> Name of the newly made GameObject </param>
        /// <param name="localPosition"> Local position of the mesh </param>
        /// <param name="localScale"> Local scale of the mesh </param>
        /// <param name="ZRotation"> Rotation on the Z-Axis </param>
        /// <param name="material"> Material used on the mesh </param>
        /// <param name="vertices"> array of vertices </param>
        /// <param name="uv"> array of UVs </param>
        /// <param name="triangles"> array of triangles </param>
        /// <param name="sortingOrder"> Sorting order of the mesh </param>
        public CustomMesh(Transform parent, string ObjectName, Vector3 localPosition, Vector3 localScale, float ZRotation, 
            Material material, Vector3[] vertices, Vector2[] uv, int[] triangles, int sortingOrder)
        {
            var mesh = new Mesh();

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            gameObject = new GameObject(ObjectName, typeof(MeshFilter), typeof(MeshRenderer));
            gameObject.transform.parent = parent;  
            gameObject.transform.localPosition = localPosition;
            gameObject.transform.localScale = localScale;
            gameObject.transform.localEulerAngles = new Vector3(0, 0, ZRotation);

            gameObject.GetComponent<MeshFilter>().mesh = mesh;
            gameObject.GetComponent<MeshRenderer>().material = material;    


        }
        #endregion

        #region Custom Class
        /// <summary>
        ///     Class containing texture part information
        /// </summary>
        public class UVCoords
        {
            public int x, y, width, height;
            public UVCoords(int x, int y, int width, int height)
            {
                this.x = x;
                this.y = y;
                this.width = width;
                this.height = height;
            }
        }
        #endregion

        #region Helper Functions
        /// <summary>
        ///     Creates a UV Rectangle (array) from the given pixels and texture.
        /// </summary>
        /// <param name="uvCoords"> UVCoords of the used texture part </param>
        /// <param name="material"> The used material </param>
        /// <returns> returns a UV Rectangle (array) </returns>
        private Vector2[] GetUVRectangleFromPixels(UVCoords uvCoords, Material material)
            => GetUVRectangleFromPixels(uvCoords.x, uvCoords.y, uvCoords.width, uvCoords.height,
                material.mainTexture.width, material.mainTexture.height);

        /// <summary>
        ///     Creates a UV Rectangle (array) from the given pixels and texture.
        /// </summary>
        /// <param name="x"> lower left X position of the texture part </param>
        /// <param name="y"> lower left Y position of the texture part </param>
        /// <param name="width"> Width of the texture part </param>
        /// <param name="height"> Height of the texture part </param>
        /// <param name="textureWidth"> Width of the full texture </param>
        /// <param name="textureHeight"> Height of the full texture </param>
        /// <returns> returns a UV Rectangle (array) </returns>
        private Vector2[] GetUVRectangleFromPixels(int x, int y, int width, int height, int textureWidth, int textureHeight)
        {
            return new Vector2[]
            {
                ConvertPixelsToUVCoordinates(x, y + height, textureWidth, textureHeight),
                ConvertPixelsToUVCoordinates(x + width, y + height, textureWidth, textureHeight),
                ConvertPixelsToUVCoordinates(x, y, textureWidth, textureHeight),
                ConvertPixelsToUVCoordinates(x + width, y, textureWidth, textureHeight)
            };
        }

        /// <summary>
        ///     Converts pixels to UV Coordinates
        /// </summary>
        /// <param name="x"> X position of the pixel </param>
        /// <param name="y"> Y position of the pixel </param>
        /// <param name="textureWidth"> Width of the texture </param>
        /// <param name="textureHeight"> Height of the texture </param>
        /// <returns> Returns the texture coordinate that should be placed on the given pixel </returns>
        private Vector2 ConvertPixelsToUVCoordinates(int x, int y, int textureWidth, int textureHeight)
            => new Vector2((float)x / textureWidth, (float)y / textureHeight);

        /// <summary>
        ///     Applies the caluclated UV array to the main UV array
        /// </summary>
        /// <param name="uv"> Calculated UV array </param>
        /// <param name="mainUV"> Main UV array </param>
        /// <exception cref="System.Exception"> Error thrown in case one of the two lists are null or do not have a length of 4 </exception>
        private void ApplyUVToUVArray(Vector2[] uv, ref Vector2[] mainUV)
        {
            if (uv == null || uv.Length < 4 || mainUV == null || mainUV.Length < 4) throw new System.Exception();
            for(int i = 0; i < 4; i++) 
                mainUV[i] = uv[i];
        }
        #endregion

    }
}
