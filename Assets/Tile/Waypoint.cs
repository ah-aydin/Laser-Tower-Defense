using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{   
    [SerializeField] bool isBuildable = false;
    public bool IsBuildable { get { return isBuildable; } }

    private TurretSelector turretSelector;

    private void Start()
    {
        turretSelector = FindObjectOfType<TurretSelector>();
    }

    private void OnMouseDown()
    {
        if (isBuildable)
        {
            // Spawn tower and mark as not buildable
            bool b_isPlaced = turretSelector.SpawnTurret(transform.position);
            isBuildable = !b_isPlaced;
        }
    }
}
