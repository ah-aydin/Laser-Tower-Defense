using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPlacer : MonoBehaviour
{
    [SerializeField] TurretPlaceholder[] turretPlaceholders;

    public bool b_isPlacingTurret { get; set; }
    private GameObject activeTurretPlaceholder = null;

    private void Awake()
    {
        b_isPlacingTurret = false;
    }

    public void initiateTurretPlacement(int turretId)
    {
        if (b_isPlacingTurret)
        {
            if (activeTurretPlaceholder)
            {
                Destroy(activeTurretPlaceholder);
            }
            b_isPlacingTurret = false;
        }
        activeTurretPlaceholder = Instantiate(turretPlaceholders[turretId].gameObject, new Vector3(0, 0, 0), Quaternion.identity);
        b_isPlacingTurret = true;
    }
}
