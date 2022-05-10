using Jonko.Grids;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PathFinding
{
    public class PathFinding 
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        private Grid<PathNode> grid;

        private List<PathNode> openList;
        private List<PathNode> closedList;

        public PathFinding(int width, int height)
        {
            grid = new Grid<PathNode>(width, height, 1f, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
        }

        public Grid<PathNode> GetGrid()
            => grid;

        public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            PathNode startNode = grid.GetGridObject(startX, startY);
            PathNode endNode = grid.GetGridObject(endX, endY);

            openList = new List<PathNode> { startNode };    
            closedList = new List<PathNode>();

            // Reset all path nodes
            for(int x = 0; x < grid.GetWidth(); x++)
            {
                for(int y = 0; y < grid.GetHeight(); y++)
                {
                    PathNode pathNode = grid.GetGridObject(x, y);
                    pathNode.gCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.cameFromNode = null;
                }
            }

            // Set all settings for he startNode
            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while(openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(openList);
                if(currentNode == endNode) return CalculatePath(endNode);
                
                openList.Remove(currentNode);
                closedList.Add(currentNode);    

                foreach(PathNode neighbourNode in GetNeighbourList(currentNode))
                {
                    if (closedList.Contains(neighbourNode)) continue;
                    if (!neighbourNode.isWalkable)
                    {
                        closedList.Add(neighbourNode);
                        continue;
                    }

                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if(tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();

                        if(!openList.Contains(neighbourNode))
                            openList.Add(neighbourNode);
                    }

                }
            }

            return null;
        }

        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            List<PathNode> neighbourList = new List<PathNode>();

            int x = currentNode.x;
            int y = currentNode.y;

            // Left side
            if(x - 1 >= 0)
            {
                // Left neighbour
                neighbourList.Add(GetNode(x - 1, y));
                // Bottom left neighbour
                if (y - 1 >= 0) neighbourList.Add(GetNode(x - 1, y - 1));
                // Top left neighbour
                if (y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(x - 1, y + 1));
            }
            // right side
            if(x + 1 < grid.GetWidth())
            {
                // Right neighbour
                neighbourList.Add(GetNode(x + 1, y));
                // Bottom right neighbour
                if (y - 1 >= 0) neighbourList.Add(GetNode(x + 1, y - 1));
                // Top right neighbour
                if (y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(x + 1, y + 1));
            }
            // Lower neighbour
            if (y - 1 >= 0) neighbourList.Add(GetNode(x, y - 1));
            // Upper neighbour
            if (y + 1 <= grid.GetHeight()) neighbourList.Add(GetNode(x,y + 1));

            return neighbourList;
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();
            PathNode currentNode = endNode;
            path.Add(endNode);
            while(currentNode.cameFromNode != null)
            {
                path.Add(currentNode.cameFromNode);
                currentNode = currentNode.cameFromNode;
            }
            path.Reverse();
            return path;
        }

        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a.x - b.x);
            int yDistance = Mathf.Abs (a.y - b.y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostNode = pathNodeList[0];
            for(int i = 1; i < pathNodeList.Count; i++)
                if(pathNodeList[i].fCost < lowestFCostNode.fCost)
                    lowestFCostNode = pathNodeList[i];
            return lowestFCostNode;
        }

        public PathNode GetNode(int x, int y)
            => grid.GetGridObject(x, y);
    }
}