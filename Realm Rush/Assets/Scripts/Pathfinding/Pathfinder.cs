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
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    Vector2Int[] directions = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};
    GridManager gridManager;

    dynamic astar;

    void Awake() {
        gridManager = FindObjectOfType<GridManager>();
        if(gridManager != null) {
            startNode = gridManager.GetNode(startCoordinates);
            destinationNode = gridManager.GetNode(destinationCoordinates);
        }
    }

    void Start()
    {
        RunPythonScript();
        gridManager.CalculateHeuristics(destinationCoordinates);
        GetNewPath();
    }

    public List<Node> GetNewPath() {
        return GetNewPath(startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coordinates) {
        CreateAstarGraph();
        gridManager.ResetNodes();
        return Astar(coordinates);
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

    List<Node> Astar(Vector2Int startCoordinates) {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        IList<object> pathCoordinates = ((IList<object>)astar.find(startCoordinates));
        Debug.Log(pathCoordinates.Count);
        foreach(object coordinates in pathCoordinates) {
            Debug.Log(coordinates);
        }
        
        /*List<(int, int)> pathCoordinates = astar.find(startCoordinates);
        List<Node> path = new List<Node>();
        foreach((int, int) tCoordinates in pathCoordinates) {
            Vector2Int coordinates = new Vector2Int(tCoordinates.Item1, tCoordinates.Item2);
            path.Add(gridManager.GetNode(coordinates));
        }*/

        return null;
    }

    void RunPythonScript() {
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

                (int, int) tCoordinates = (x, y);
                astar.add_heuristic(tCoordinates, currentNode.heuristic);
                
                foreach(Vector2Int direction in directions) {
                    
                    Vector2Int neighborCoordinates = currentNode.coordinates + direction;
                    Node neighborNode = gridManager.GetNode(neighborCoordinates);
                    if(neighborNode == null || !neighborNode.isWalkable) continue; 

                    (int, int) tNeighborCoordinates = (neighborCoordinates.x
                                                        ,neighborCoordinates.y);
                    astar.add_edge(tCoordinates, tNeighborCoordinates, neighborNode.cost);
                }
            }
        }
    }

    public void NotifyReceivers() {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }

}
