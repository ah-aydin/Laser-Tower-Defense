using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretPlacementButton : MonoBehaviour
{
    [Tooltip("Set this to the turret index you want to spawn set in the TurretPlacer")]
    [SerializeField] int turretId = 0;

    private TurretPlacer turretPlacer;
    private Button button; 

    private void Awake()
    {
        button = GetComponent<Button>();
        turretPlacer = FindObjectOfType<TurretPlacer>();

        button.onClick.AddListener(delegate { turretPlacer.initiateTurretPlacement(turretId); } );
    }
}
