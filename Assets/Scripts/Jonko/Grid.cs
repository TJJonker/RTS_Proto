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
                        GetWorldPosition(x, y) + Vector3.one * cellSize * .5f, 4, TextAnchor.MiddleCenter, TextAlignment.Center);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100);

            SetValue(4, 7, 69);
        }

        private Vector3 GetWorldPosition(int x, int y) => new Vector3(x, y) * cellSize;

        public void SetValue(int x, int y, int value)
        {
            if(x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x,y] = value;
                debugTextArray[x,y].text = gridArray[x, y].ToString();
            }
        }
    }
}
