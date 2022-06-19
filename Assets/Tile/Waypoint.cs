using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] Turret turretPrefab;
    
    [SerializeField] bool isBuildable = false;
    public bool IsBuildable { get { return isBuildable; } }

    private void OnMouseDown()
    {
        if (isBuildable)
        {
            // Spawn tower and mark as not buildable
            bool b_isPlaced = turretPrefab.CreateTurret(turretPrefab, transform.position);
            isBuildable = !b_isPlaced;
        }
    }
}
