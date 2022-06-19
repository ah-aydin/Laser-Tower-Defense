using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform rotator;

    [SerializeField] float range = 20f;
    public float Range { get { return range; }}

    private ObjectPool enemyPool;                       // Reference to the enemy pool

    private Transform target = null;                    // Reference to the target
    public Transform Target { get { return target; }}

    private void Awake()
    {
        enemyPool = FindObjectOfType<ObjectPool>();
    }

    private void Update()
    {
        // If there is a target and it is active
        if (target && target.gameObject.activeInHierarchy)
        {
            // Check if it is within range, if so attack
            if (Vector3.Distance(transform.position, target.position) < range)
            {
                AimWeapon();
                return;
            }
            else
            {
                target = null;
            }
        }

        // If there is no target within range, find new target
        FindTarget();
    }

    private void FindTarget()
    {
        GameObject closestEnemy = null;
        float maxDistance = Mathf.Infinity;

        GameObject[] enemies = enemyPool.GetActiveEnemies();
        if (enemies.Length <= 0) 
        {
            target = null;
            return;
        }

        // Get list of active enemies
        foreach (GameObject enemy in enemies)
        {
            // Check the distances and choose the closest one
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            
            // Ignore if it is out of range
            if (distance > range) continue;

            // Replace the closes enemy if it is closer than the stored one
            if (distance < maxDistance)
            {
                maxDistance = distance;
                closestEnemy = enemy;
            }
        }

        // Set as new target if found
        if (closestEnemy) target = closestEnemy.transform;
    }

    private void AimWeapon()
    {
        rotator.LookAt(target.transform);
    }
}
