using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPlaceholder : MonoBehaviour
{
    [Tooltip("Set this to the layer mask that the tiles are set to.")]
    [SerializeField] LayerMask tileMask = 10;
    [Tooltip("Set this to the turret prefabe that must be constructed")]
    [SerializeField] Turret turretPrefab;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private float maxRaycastDistance = 500;

    private TurretPlacer turretPlacer;

    private void Awake()
    {
        turretPlacer = FindObjectOfType<TurretPlacer>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        FollowMouseLocationOnGrid();
        HandlePlaceTurret();
    }

    private void FollowMouseLocationOnGrid()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, maxRaycastDistance, tileMask))
        {
            transform.position = hit.collider.transform.position;
        }
    }

    private void HandlePlaceTurret()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0)) return;
        
        // Get the tile from the last hit component and place try to place the turret
        Tile tile = hit.collider.GetComponent<Tile>();
        if (!tile) return;
        bool b_isPlaced = tile.HandleTurretPlacement(turretPrefab);
        if (b_isPlaced)
        {
            turretPlacer.b_isPlacingTurret = false;
            Destroy(gameObject);
        }
    }
}
