using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform rotator;
    [SerializeField] Transform target;

    private void Start()
    {
        target = FindObjectOfType<EnemyMover>().transform;
    }

    private void Update()
    {
        AimWeapon();
    }

    void AimWeapon()
    {
        rotator.LookAt(target.transform);
    }
}
