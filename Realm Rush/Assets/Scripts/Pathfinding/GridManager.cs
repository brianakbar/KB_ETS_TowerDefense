using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    public Vector2Int GridSize { get { return gridSize; } }

    [SerializeField] int unityGridSize = 10;
    public int UnityGridSize { get { return unityGridSize; } }
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } }

    void Awake() {
        CreateGrid();
    }

    public Node GetNode(Vector2Int coordinates) {
        if(grid.ContainsKey(coordinates)) {
            return grid[coordinates];
        }
        return null;
    }

    public void AddCostToNode(Vector2Int coordinates, int cost) {
        if(grid.ContainsKey(coordinates)) {
            grid[coordinates].cost = cost;
        }
    }

    public void AddHeuristicToNode(Vector2Int coordinates, int heuristic) {
        if(grid.ContainsKey(coordinates)) {
            grid[coordinates].heuristic = heuristic;
        }
    }

    public void BlockNode(Vector2Int coordinates) {
        if(grid.ContainsKey(coordinates)) {
           grid[coordinates].isWalkable = false;
        }
    }

    public void ResetNodes() {
        foreach(KeyValuePair<Vector2Int, Node> entry in grid) {
            entry.Value.connectedTo = null;
            entry.Value.isExplored = false;
            entry.Value.isPath = false;
        }
    }

    public Vector2Int GetCoordinatesFromPosition(Vector3 position) {
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x / UnityGridSize);
        coordinates.y = Mathf.RoundToInt(position.z / UnityGridSize);
        return coordinates;
    }

    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates) {
        Vector3 position = new Vector3();
        position.x = coordinates.x * UnityGridSize;
        position.z = coordinates.y * UnityGridSize;
        return position;
    }

    public void CalculateHeuristics(Vector2Int destinationCoordinates) {
        for(int x = 0; x < gridSize.x; x++) {
            for(int y = 0; y < gridSize.y; y++) {
                int heuristic = Mathf.Abs(x - destinationCoordinates.x) 
                                + Mathf.Abs(y - destinationCoordinates.y);
                Vector2Int coordinates = new Vector2Int(x, y);
                AddHeuristicToNode(coordinates, heuristic);
            }
        }
    }

    void CreateGrid() {
        for(int x = 0; x < gridSize.x; x++) {
            for(int y = 0; y < gridSize.y; y++) {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new Node(coordinates, true));
            }
        }
    }
}
