using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways] 
[RequireComponent(typeof(TMP_Text))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.black;
    [SerializeField] Color blockedColor = Color.gray;
    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathColor = new Color(1, 0.6f, 0);

    TMP_Text label;
    Vector2Int coordinates;
    GridManager gridManager;

    void Awake() {
        label = GetComponent<TMP_Text>();
        label.enabled = false;
        gridManager = FindObjectOfType<GridManager>();
        DisplayCoordinates();
    }

    void Update()
    {
        if (!Application.isPlaying) {
            DisplayCoordinates();
            UpdateObjectName();
            label.enabled = true;
        }

        SetLabelColor();
        ToggleLabels();
    }

    void DisplayCoordinates() {
        if(gridManager == null) { return; }
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSize);

        label.text = coordinates.x + "," + coordinates.y;
    }

    void UpdateObjectName() {
        transform.parent.name = coordinates.ToString();
    }

    void SetLabelColor() {
        if(gridManager == null) { return; }
        Node node = gridManager.GetNode(coordinates);
        if(node == null) { return; }

        if(!node.isWalkable) {
            label.color = blockedColor;
        }
        else if(node.isPath) {
            label.color = pathColor;
        }
        else if(node.isExplored) {
            label.color = exploredColor;
        }
        else {
            label.color = defaultColor;
        }
    }

    void ToggleLabels() {
        if(Input.GetKeyDown(KeyCode.C)) {
            label.enabled = !label.enabled;
        }
    }
}
