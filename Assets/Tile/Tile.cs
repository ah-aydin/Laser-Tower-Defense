using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{   
    [SerializeField] bool isBuildable = false;
    public bool IsBuildable { get { return isBuildable; } }

    private TurretSelector turretSelector;
    private GridManager gridManager;
    private Pathfinder pathfinder;
    private Vector2Int coordinates;

    private void Awake()
    {
        turretSelector = FindObjectOfType<TurretSelector>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
        if (gridManager)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
            if (!isBuildable)
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }

    private void Start()
    {
        if (!isBuildable)
        {
            gridManager.BlockNode(coordinates);
        }
    }

    public bool HandleTurretPlacement(Turret turretPrefab)
    {
        if (isBuildable && gridManager.GetNode(coordinates).isWalkable && !pathfinder.WillBlockPath(coordinates))
        {
            // Spawn turret and mark as not buildable
            bool b_isPlaced = turretPrefab.CreateTurret(turretPrefab, transform.position);
            isBuildable = !b_isPlaced;
            if (b_isPlaced)
            {
                gridManager.BlockNode(coordinates);
                pathfinder.NotifyReceivers();
                return true;
            }
        }
        return false;
    }
}
