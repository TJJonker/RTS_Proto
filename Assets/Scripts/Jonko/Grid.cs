using UnityEngine;

namespace Jonko.Grids {
    public class Grid
    {
        private int width;
        private int height;
        private float cellSize;
        private int[,] gridArray;
        private TextMesh[,] debugTextArray;

        public Grid(int width, int height, float cellSize)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;


            gridArray = new int[width, height];
            debugTextArray = new TextMesh[width, height];

            for(int x = 0; x < gridArray.GetLength(0); x++)
            {
                for(int y = 0; y < gridArray.GetLength(1); y++)
                {
                    debugTextArray[x, y] = Visualisation.Visualisation.CreateWorldText(gridArray[x,y].ToString(), Color.white, null, 
                        GetWorldPosition(x, y) + Vector2.one * cellSize * .5f, 4, TextAnchor.MiddleCenter, TextAlignment.Center);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100);

            SetValue(4, 7, 69);
        }

        /// <summary>
        ///     Converts grid position to world position
        /// </summary>
        /// <param name="position"> Vector2 grid position </param>
        /// <returns></returns>
        private Vector2 GetWorldPosition(Vector2 position)
            => position * cellSize;

        /// <summary>
        ///     Converts grid position to world position
        /// </summary>
        /// <param name="x"> X position on the grid </param>
        /// <param name="y"> Y position on the grid </param>
        /// <returns></returns>
        private Vector2 GetWorldPosition(int x, int y) 
            => GetWorldPosition(new Vector2(x, y));

        /// <summary>
        ///     Converts world position to grid position
        /// </summary>
        /// <param name="position"> World position </param>
        /// <param name="x"> X position on the grid </param>
        /// <param name="y"> Y position on the grid </param>
        private void GetGridPosition(Vector2 position, out int x, out int y)
        {
            x = Mathf.FloorToInt(position.x / cellSize);
            y = Mathf.FloorToInt(position.y / cellSize);
        }

        /// <summary>
        ///     Converts world position to grid position
        /// </summary>
        /// <param name="posX"> X position in the world </param>
        /// <param name="posY"> Y position in the world </param>
        /// <param name="x"> X position on the grid </param>
        /// <param name="y"> Y position on the grid </param>
        private void GetGridPosition(int posX, int posY, out int x, out int y)
            => GetGridPosition(new Vector2(posX, posY), out x, out y);

        /// <summary>
        ///     Sets the value a specific place in the grid
        /// </summary>
        /// <param name="x"> X position on the grid </param>
        /// <param name="y"> Y position on the grid </param>
        /// <param name="value"> Value to put in the grid spot </param>
        public void SetValue(int x, int y, int value)
        {
            if(x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x,y] = value;
                debugTextArray[x,y].text = gridArray[x, y].ToString();
            }
        }

        /// <summary>
        ///     Sets the value a specific place in the grid
        /// </summary>
        /// <param name="position"> Position on the grid </param>
        /// <param name="value"> Value to put in the grid spot </param>
        public void SetValue(Vector3 position, int value)
        {
            int x, y;
            GetGridPosition(position, out x, out y);
            SetValue(x, y, value);
        }


    }
}
