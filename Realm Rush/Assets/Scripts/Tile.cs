using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;
    [SerializeField] int cost = 1;

    [SerializeField] bool isPlaceable;
    public bool IsPlaceable { get { return isPlaceable; } }

    GridManager gridManager;
    Pathfinder[] pathfinder;
    Vector2Int coordinates = new Vector2Int();

    void Awake() {
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectsOfType<Pathfinder>();
    }

    void Start() {
        if(gridManager != null) {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
            if(!isPlaceable) {
                gridManager.BlockNode(coordinates);
            }
            gridManager.AddCostToNode(coordinates, cost);
        }
    }

    void OnMouseDown() {
        if(gridManager.GetNode(coordinates).isWalkable && !WillBlockPaths()) {
            bool isPlaced = towerPrefab.CreateTower(transform.position);
            if(isPlaced) {
                gridManager.BlockNode(coordinates);
                for(int i = 0; i < pathfinder.Length; i++) {
                    pathfinder[i].NotifyReceivers();
                }
            }
        }
    }

    bool WillBlockPaths() {
        for(int i = 0; i < pathfinder.Length; i++) {
            if(pathfinder[i].WillBlockPath(coordinates)) {
                return true;
            }
        }
        return false;
    }
}
