using Jonko.Grids;

namespace Game.PathFinding
{
    public class PathNode
    {
        private Grid<PathNode> grid;
        public int x, y;

        public int gCost, hCost, fCost;
        public PathNode cameFromNode;


        public PathNode(Grid<PathNode> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void CalculateFCost()
            => fCost = gCost + hCost;

        public override string ToString()
            => x + "," + y;

    }
}
