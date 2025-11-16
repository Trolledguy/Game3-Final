using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Simple grid-based pathfinding system
/// Uses A* algorithm for finding paths without NavMesh
/// </summary>
public class PathfindingGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private Vector3 gridOrigin = Vector3.zero;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private int gridWidth = 50;
    [SerializeField] private int gridHeight = 50;
    [SerializeField] private LayerMask obstacleLayer;
    
    [Header("Raycast Settings")]
    [SerializeField] private float cellCheckRadius = 0.4f;
    
    private GridNode[,] grid;
    private bool isInitialized = false;

    private void Start()
    {
        InitializeGrid();
    }

    /// <summary>
    /// Initialize the pathfinding grid
    /// </summary>
    public void InitializeGrid()
    {
        if (isInitialized) return;
        
        grid = new GridNode[gridWidth, gridHeight];
        
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 worldPos = GetWorldPosition(x, y);
                bool isWalkable = !Physics.CheckSphere(worldPos, cellCheckRadius, obstacleLayer);
                grid[x, y] = new GridNode(x, y, isWalkable);
            }
        }
        
        isInitialized = true;
    }

    /// <summary>
    /// Find a path from start to end position using A*
    /// </summary>
    public List<Vector3> FindPath(Vector3 startPos, Vector3 endPos)
    {
        if (!isInitialized) InitializeGrid();
        
        GridNode startNode = GetNode(startPos);
        GridNode endNode = GetNode(endPos);
        
        if (startNode == null || endNode == null || !endNode.isWalkable)
            return new List<Vector3> { endPos };
        
        List<GridNode> openSet = new List<GridNode>();
        HashSet<GridNode> closedSet = new HashSet<GridNode>();
        
        openSet.Add(startNode);
        
        while (openSet.Count > 0)
        {
            int current = 0;
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < openSet[current].fCost)
                    current = i;
            }
            
            GridNode currentNode = openSet[current];
            
            if (currentNode == endNode)
                return RetracePath(startNode, endNode);
            
            openSet.RemoveAt(current);
            closedSet.Add(currentNode);
            
            foreach (GridNode neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.isWalkable || closedSet.Contains(neighbor))
                    continue;
                
                float newG = currentNode.gCost + Vector3.Distance(
                    GetWorldPosition(currentNode.x, currentNode.y),
                    GetWorldPosition(neighbor.x, neighbor.y));
                
                bool newPath = false;
                if (openSet.Contains(neighbor))
                {
                    if (newG < neighbor.gCost)
                    {
                        neighbor.gCost = newG;
                        newPath = true;
                    }
                }
                else
                {
                    neighbor.gCost = newG;
                    newPath = true;
                    openSet.Add(neighbor);
                }
                
                if (newPath)
                {
                    neighbor.hCost = Vector3.Distance(
                        GetWorldPosition(neighbor.x, neighbor.y),
                        endPos);
                    neighbor.parent = currentNode;
                }
            }
        }
        
        // No path found, return end position
        return new List<Vector3> { endPos };
    }

    private List<Vector3> RetracePath(GridNode startNode, GridNode endNode)
    {
        List<Vector3> path = new List<Vector3>();
        GridNode current = endNode;
        
        while (current != startNode)
        {
            path.Add(GetWorldPosition(current.x, current.y));
            current = current.parent;
        }
        
        path.Reverse();
        return path;
    }

    private List<GridNode> GetNeighbors(GridNode node)
    {
        List<GridNode> neighbors = new List<GridNode>();
        
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                
                int newX = node.x + x;
                int newY = node.y + y;
                
                if (newX >= 0 && newX < gridWidth && newY >= 0 && newY < gridHeight)
                    neighbors.Add(grid[newX, newY]);
            }
        }
        
        return neighbors;
    }

    private GridNode GetNode(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - gridOrigin.x) / cellSize);
        int z = Mathf.FloorToInt((worldPos.z - gridOrigin.z) / cellSize);
        
        if (x >= 0 && x < gridWidth && z >= 0 && z < gridHeight)
            return grid[x, z];
        
        return null;
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return gridOrigin + new Vector3(x * cellSize + cellSize / 2f, 0, y * cellSize + cellSize / 2f);
    }

    public void OnDrawGizmos()
    {
        if (!isInitialized) return;
        
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 pos = GetWorldPosition(x, y);
                Gizmos.color = grid[x, y].isWalkable ? Color.white : Color.red;
                Gizmos.DrawCube(pos, Vector3.one * (cellSize * 0.8f));
            }
        }
    }
}

public class GridNode
{
    public int x, y;
    public bool isWalkable;
    public float gCost; // Cost from start
    public float hCost; // Heuristic cost to end
    public GridNode parent;
    
    public float fCost => gCost + hCost;

    public GridNode(int x, int y, bool isWalkable)
    {
        this.x = x;
        this.y = y;
        this.isWalkable = isWalkable;
        this.gCost = 0;
        this.hCost = 0;
        this.parent = null;
    }
}
