using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; }}
    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DestinationsCoordinates { get { return destinationCoordinates; }}

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();
    Queue<Node> frontier = new Queue<Node>();

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null)
        {
            grid = gridManager.Grid;
            startNode = grid[startCoordinates];
            destinationNode = grid[destinationCoordinates];
        }
    }

    private void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNodes();
        bool reachedEnd = BreadthFirstSearch(coordinates);
        return BuildPath(reachedEnd);
    }

    private void ExploreNeighbors()
    {
        // Find the neighbors of the current node
        List<Node> neighbors = new List<Node>();
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborCoords = currentSearchNode.coordinates + direction;
            if (grid.ContainsKey(neighborCoords))
            {
                if (!grid[neighborCoords].isWalkable) continue;
                neighbors.Add(grid[neighborCoords]);
            }
        }

        // Process the neighbors
        foreach(Node neighbor in neighbors)
        {
            if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                neighbor.connectedTo = currentSearchNode;
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    private bool BreadthFirstSearch(Vector2Int coordinates)
    {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear();
        reached.Clear();

        bool isRunning = true;

        frontier.Enqueue(grid[coordinates]);
        reached.Add(coordinates, grid[coordinates]);

        while (frontier.Count > 0 && isRunning)
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if (currentSearchNode.coordinates == destinationCoordinates)
            {
                isRunning = false;
                return true;
            }
        }
        return false;
    }

    // Backtraces the shortes path from the destination node to the start node
    private List<Node> BuildPath(bool reachedEnd)
    {
        if (!reachedEnd) return new List<Node>();

        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;
        
        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)
        {
            path.Add(currentNode.connectedTo);
            currentNode = currentNode.connectedTo;
            currentNode.isPath = true;
        }
        path.Add(currentNode);
        path.Reverse();

        return path;
    }

    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (!grid.ContainsKey(coordinates)) return false;

        // If it is either the start or the end tile, do not allow
        if (coordinates == startCoordinates || coordinates == destinationCoordinates) return true;

        bool prevState = grid[coordinates].isWalkable;
        grid[coordinates].isWalkable = false;
        List<Node> newPath = GetNewPath();
        grid[coordinates].isWalkable = prevState;
        
        if (newPath.Count == 0)
        {
            GetNewPath();
            return true;
        }
        return false;
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}
