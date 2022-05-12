using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Game.ECSPathFinding {
    public class ECSPathFinding : MonoBehaviour
    {
        private const int MOVE_DIAGONAL_COST = 14;
        private const int MOVE_STRAIGHT_COST = 10;

        private void Start()
        {
            FindPath(new int2(0, 0), new int2(1, 2));
        }

        private void FindPath(int2 startPosition, int2 endPosition)
        {
            int2 gridSize = new int2(3, 3);

            NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.Temp);

            for(int x = 0; x < gridSize.x; x++) {
                for(int y = 0; y < gridSize.y; y++) {
                    PathNode pathNode = new PathNode();
                    pathNode.x = x;
                    pathNode.y = y;
                    pathNode.index = CalculateIndex(x, y, gridSize.x);

                    pathNode.gCost = int.MaxValue;
                    pathNode.hCost = CalculateDistanceCost(new int2(x, y), endPosition);
                    pathNode.CalculateFCost();

                    pathNode.isWalkable = true;
                    pathNode.cameFromNodeIndex = -1;

                    pathNodeArray[pathNode.index] = pathNode;
                }
            }

            NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(new int2[]
            {
                new int2(-1, +0), // Left
                new int2(-1, +1), // Top left
                new int2(+0, +1), // Top
                new int2(+1, +1), // Top right
                new int2(+1, +0), // Right
                new int2(+1, -1), // Bottom right
                new int2(+0, -1), // Bottom
                new int2(-1, -1), // Bottom left
            }, Allocator.Temp);

            int endNodeIndex = CalculateIndex(endPosition.x, endPosition.y, gridSize.x);

            PathNode startNode = pathNodeArray[CalculateIndex(startPosition.x, startPosition.y, gridSize.x)];
            startNode.gCost = 0;
            startNode.CalculateFCost();
            pathNodeArray[startNode.index] = startNode;

            NativeList<int> openList = new NativeList<int>(Allocator.Temp);
            NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

            openList.Add(startNode.index);

            while (openList.Length > 0)
            {
                int currentNodeIndex = GetLowestFCostNodeIndex(openList, pathNodeArray);
                PathNode currentNode = pathNodeArray[currentNodeIndex];

                if (currentNodeIndex == endNodeIndex) break;
                for(int i = 0; i < openList.Length; i++)
                {
                    if (openList[i] == currentNodeIndex)
                    {
                        openList.RemoveAtSwapBack(i);
                        break;
                    }
                }

                closedList.Add(currentNodeIndex);   

                for(int i = 0; i < neighbourOffsetArray.Length; i++)
                {
                    int2 neighbourOffset = neighbourOffsetArray[i];
                    int2 neighbourPosition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);

                    if(!isPositionInsideGrid(neighbourPosition, gridSize)) continue;

                    int neighbourNodeIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y, gridSize.x);
                    if (closedList.Contains(neighbourNodeIndex)) continue;

                    PathNode neighbourNode = pathNodeArray[neighbourNodeIndex];
                    if(!neighbourNode.isWalkable) continue;

                    int2 currentNodePosition = new int2(currentNode.x, currentNode.y);
                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighbourPosition);
                    if(tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNodeIndex = currentNodeIndex;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.CalculateFCost();
                        pathNodeArray[neighbourNodeIndex] = neighbourNode;

                        if(!openList.Contains(neighbourNode.index))
                            openList.Add(neighbourNode.index);
                    }
                }
            }

            PathNode endNode = pathNodeArray[endNodeIndex];
            if(endNode.cameFromNodeIndex == -1)
            {
                Debug.Log("Did not find a path");
            }
            else
            {
                NativeList<int2> path = CalculatePath(pathNodeArray, endNode);
                foreach (int2 pathPosition in path)
                    Debug.Log(pathPosition);
                path.Dispose();
            }

            pathNodeArray.Dispose();
            openList.Dispose();
            closedList.Dispose();
            neighbourOffsetArray.Dispose();
        }

        private NativeList<int2> CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode)
        {
            if (endNode.cameFromNodeIndex == -1)
            {
                // Couldn't find a path!
                return new NativeList<int2>(Allocator.Temp);
            }
            else
            {
                // Found a path
                NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
                path.Add(new int2(endNode.x, endNode.y));

                PathNode currentNode = endNode;

                
                while (currentNode.cameFromNodeIndex != -1)
                {
                    PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex];
                    path.Add(new int2(cameFromNode.x, cameFromNode.y));
                    currentNode = cameFromNode;
                }
                
                return path;
            }
        }

        private bool isPositionInsideGrid(int2 gridPosition, int2 gridSize)
        {
            return
                gridPosition.x >= 0 &&
                gridPosition.y >= 0 &&
                gridPosition.x < gridSize.x &&
                gridPosition.y < gridSize.y;
        }

        private int GetLowestFCostNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray)
        {
            PathNode lowestCostPathNode = pathNodeArray[openList[0]];
            for(int i = 1; i < openList.Length; i++)
            {
                PathNode testPathNode = pathNodeArray[openList[i]];
                if(testPathNode.fCost < lowestCostPathNode.fCost)
                    lowestCostPathNode = testPathNode;
            }
            return lowestCostPathNode.index;
        }

        private int CalculateDistanceCost(int2 startPosition, int2 endPosition)
        {
            int xDistance = Mathf.Abs(startPosition.x - endPosition.x);
            int yDistance = Mathf.Abs(startPosition.y - endPosition.y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private int CalculateIndex(int x, int y, int gridWidth)
            => x + y * gridWidth;

        private struct PathNode
        {
            public int x, y;

            public int index;

            public int gCost;
            public int hCost;
            public int fCost;

            public bool isWalkable;

            public int cameFromNodeIndex;

            public void CalculateFCost()
                => fCost = gCost + hCost;
        }
    }
}
