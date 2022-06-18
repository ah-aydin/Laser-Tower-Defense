using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] GameObject turretPrefab;
    [SerializeField] bool isBuildable = false;

    private void OnMouseDown()
    {
        if (isBuildable)
        {
            // Spawn tower and mark as not buildable
            Instantiate(turretPrefab, transform.position, Quaternion.identity);
            isBuildable = false;
        }
    }
}
