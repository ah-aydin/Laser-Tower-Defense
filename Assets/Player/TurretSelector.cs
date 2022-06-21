using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSelector : MonoBehaviour
{
    [Tooltip("Put the turret prefabs here. The UI should reflect what is being put here.")]
    [SerializeField] List<Turret> turretPrefabs;

    private Turret selectedTurret;

    private void Start()
    {
        selectedTurret = turretPrefabs[0];
    }

    public void ChangeSelectedTurret(int index)
    {
        selectedTurret = turretPrefabs[index];
    }

    public bool SpawnTurret(Vector3 transformPosition)
    {
        return selectedTurret.CreateTurret(selectedTurret, transformPosition);
    }
}
