using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Executes in play and edit mode
[ExecuteAlways]
public class CoordinateLable : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;
    [SerializeField] string tileType = "Standard";

    private TextMeshPro label;
    private Waypoint waypoint;
    private Vector2Int coordinates = new Vector2Int();

    private void Awake()
    {
        // Switch label off by default
        label = GetComponent<TextMeshPro>();
        label.enabled = false;

        waypoint = GetComponentInParent<Waypoint>();
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
        ColorCoordinates();
        ToggleLabels();
    }

    private void EditorFunctionality()
    {
        DisplayCoordinates();
        UpdateObjectName();
    }

    private void ColorCoordinates()
    {
        if (waypoint.IsBuildable)
        {
            label.color = defaultColor;
        }
        else
        {
            label.color = blockedColor;
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
