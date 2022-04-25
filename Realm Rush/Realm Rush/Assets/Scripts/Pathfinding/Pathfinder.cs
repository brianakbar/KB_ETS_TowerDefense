using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IronPython.Hosting;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates {get { return startCoordinates; } }

    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates {get { return destinationCoordinates; } }

    Node startNode;
    Node destinationNode;

    Vector2Int[] directions = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};
    GridManager gridManager;

    dynamic astar;

    void Awake() {
        gridManager = FindObjectOfType<GridManager>();
        if(gridManager != null) {
            startNode = gridManager.GetNode(startCoordinates);
            destinationNode = gridManager.GetNode(destinationCoordinates);
        }
        RunAstarScript();
    }

    void Start()
    {
        gridManager.CalculateHeuristics(destinationCoordinates);
        GetNewPath();
    }

    public List<Node> GetNewPath() {
        return GetNewPath(startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coordinates) {
        CreateAstarGraph();
        gridManager.ResetNodes();
        Astar(coordinates);
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

    void Astar(Vector2Int startCoordinates) {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        IList<object> exploredCoordinates = ((IList<object>)astar.find(startCoordinates));
        
        foreach(object coordinates in exploredCoordinates) {
            gridManager.GetNode((Vector2Int)coordinates).isExplored = true;
        }
    }

    List<Node> BuildPath() {
        IList<object> pathCoordinates = ((IList<object>)astar.get_path());
        List<Node> path = new List<Node>();

        foreach(object coordinates in pathCoordinates) {
            Node pathNode = gridManager.GetNode((Vector2Int)coordinates);
            pathNode.isPath = true;
            path.Add(pathNode);
        }

        return path;
    }

    void RunAstarScript() {
        var engine = Python.CreateEngine ();
		ICollection<string> searchPaths = engine.GetSearchPaths ();

        searchPaths.Add(Application.dataPath);
        searchPaths.Add(Application.dataPath + @"\StreamingAssets"  + @"\Lib\");
		engine.SetSearchPaths(searchPaths);

		dynamic py = engine.ExecuteFile(Application.dataPath + @"\StreamingAssets" + @"\Python\astar.py");
        astar = py.Graph();
    }

    void CreateAstarGraph() {
        if(astar == null) return;
        if(gridManager == null) return;

        astar.clear_graph();
        for(int x = 0; x < gridManager.GridSize.x; x++) {
            for(int y = 0; y < gridManager.GridSize.y; y++) {
                Vector2Int coordinates = new Vector2Int(x, y);
                Node currentNode = gridManager.GetNode(coordinates);
                if(currentNode == null || !currentNode.isWalkable) continue;

                astar.add_heuristic(coordinates, currentNode.heuristic);
                foreach(Vector2Int direction in directions) {
                    
                    Vector2Int neighborCoordinates = currentNode.coordinates + direction;
                    Node neighborNode = gridManager.GetNode(neighborCoordinates);
                    if(neighborNode == null || !neighborNode.isWalkable) continue; 

                    astar.add_edge(coordinates, neighborCoordinates, neighborNode.cost);
                }
            }
        }
    }

    public void NotifyReceivers() {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }

}
