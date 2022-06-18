using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Executes in play and edit mode
[ExecuteAlways]
public class CoordinateLable : MonoBehaviour
{
    [SerializeField] string tileType = "Standard";

    private TextMeshPro label;
    private Vector2Int coordinates = new Vector2Int();

    private void Awake()
    {
        label = GetComponent<TextMeshPro>();
        DisplayCoordinates();
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            EditorFunctionality();
            return;
        }
    }

    private void EditorFunctionality()
    {
        DisplayCoordinates();
        UpdateObjectName();
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
