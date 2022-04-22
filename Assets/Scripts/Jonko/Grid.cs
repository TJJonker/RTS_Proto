using UnityEngine;

namespace Jonko.Grids {
    public class Grid
    {
        private int width;
        private int height;
        private int[,] gridArray;

        public Grid(int width, int height, float cellSize)
        {
            this.width = width;
            this.height = height;

            gridArray = new int[width, height];

            for(int x = 0; x < gridArray.GetLength(0); x++)
            {
                for(int y = 0; y < gridArray.GetLength(1); y++)
                {
                    Visualisation.Visualisation.CreateWorldText("yay", Color.white);
                }
            }
        }
    }
}
