using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder1 : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates {get { return startCoordinates; } }

    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates {get { return destinationCoordinates; } }

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    Vector2Int[] directions = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};
    GridManager gridManager;

    void Awake() {
        gridManager = FindObjectOfType<GridManager>();
        if(gridManager != null) {
            startNode = gridManager.GetNode(startCoordinates);
            destinationNode = gridManager.GetNode(destinationCoordinates);
        }
    }

    void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath() {
        return GetNewPath(startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coordinates) {
        gridManager.ResetNodes();
        BreadthFirstSearch(coordinates);
        return BuildPath();
    }

    public bool WillBlockPath(Vector2Int coordinates) {
        Node blockedNode = gridManager.GetNode(coordinates);
        if(blockedNode != null) {
            bool previousState = blockedNode.isWalkable;

            blockedNode.isWalkable = false;
            List<Node> newPath = GetNewPath();
            blockedNode.isWalkable = previousState;

            if(newPath.Count <= 1) {
                GetNewPath();
                return true;
            }
        }
        return false;
    }

    void ExploreNeighbors() {
        if(gridManager == null) { return; }

        List<Node> neighbors = new List<Node>();
        foreach(Vector2Int direction in directions) {
            Vector2Int neighborCoordinates = currentSearchNode.coordinates + direction;
            Node neighbor = gridManager.GetNode(neighborCoordinates);
            if(neighbor != null) {
                neighbors.Add(neighbor);
            }
        }

        foreach(Node neighbor in neighbors) {
            if(!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable) {
                neighbor.connectedTo = currentSearchNode;
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    void BreadthFirstSearch(Vector2Int startCoordinates) {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear();
        reached.Clear();

        bool isRunning = true;
        startNode = gridManager.GetNode(startCoordinates);
        frontier.Enqueue(startNode);
        reached.Add(startCoordinates, startNode);

        while(frontier.Count > 0 && isRunning) {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if(currentSearchNode.coordinates == destinationCoordinates) {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath() {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        while(currentNode != null) {
            path.Add(currentNode);
            currentNode.isPath = true;
            currentNode = currentNode.connectedTo;
        }

        path.Reverse();
        return path;
    }

    public void NotifyReceivers() {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }

}
