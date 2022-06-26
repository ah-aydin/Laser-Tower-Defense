using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Executes in play and edit mode
[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLable : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color walkableColor = Color.gray;
    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathColor = new Color(1f, 0.5f, 0f);
    [SerializeField] string tileType = "Standard";

    private TextMeshPro label;
    private Vector2Int coordinates = new Vector2Int(0, 0);
    GridManager gridManager;

    private void Awake()
    {
        // Switch label off by default
        label = GetComponent<TextMeshPro>();
        //label.enabled = false;

        gridManager = FindObjectOfType<GridManager>();

        DisplayCoordinates();
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            EditorFunctionality();
            return;
        }

        // Debuging
        SetLabelColor();
        ToggleLabels();
    }

    private void EditorFunctionality()
    {
        DisplayCoordinates();
        UpdateObjectName();
    }

    private void SetLabelColor()
    {
        if (gridManager == null) return;

        Node node = gridManager.GetNode(coordinates);
       
        if (node == null) return;
        
        if (node.isPath)
        {
            label.color = pathColor;
        }
        else if (node.isExplored)
        {
            label.color = exploredColor;
        }
        else if (node.isWalkable)
        {
            label.color = walkableColor;
        }
        else
        {
            label.color = defaultColor;
        }
    }

    void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            label.enabled = !label.enabled;
        }
    }

    private void DisplayCoordinates()
    {
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x) ;
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);

        label.text = $"{coordinates.x}.{coordinates.y}";
    }

    private void UpdateObjectName()
    {
        transform.parent.name = $"{tileType} {coordinates.ToString()}";
    }
}
