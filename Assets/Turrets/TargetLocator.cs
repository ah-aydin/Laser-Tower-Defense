using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform rotator;
    [SerializeField] float range = 20f;

    private ObjectPool enemyPool;                       // Reference to the enemy pool

    private Transform target;                           // Reference to the target
    private ParticleSystem[] weapionParticleSystems;    // Reference to the weapon particle systems
    private bool b_isFiring = false;

    private Animator animator;
    private string ANIM_IS_FIRING = "IsFiring";

    private void Awake()
    {
        enemyPool = FindObjectOfType<ObjectPool>();

        animator = GetComponent<Animator>();
        weapionParticleSystems = GetComponentsInChildren<ParticleSystem>();

        // Disable weapons by default
        HandleWeapons(false);
    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        // If there is a target and it is active
        if (target && target.gameObject.activeInHierarchy)
        {
            // Check if it is within range, if so attack
            if (Vector3.Distance(transform.position, target.position) < range)
            {
                AimWeapon();
                Fire(true);
                return;
            }
        }

        // If there is no target within range, stop firing and find new target
        Fire(false);
        FindTarget();
    }

    private void FindTarget()
    {
        GameObject closestEnemy = null;
        float maxDistance = Mathf.Infinity;

        // Get list of active enemies
        foreach (GameObject enemy in enemyPool.GetActiveEnemies())
        {
            // Check the distances and choose the closest one
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < maxDistance)
            {
                maxDistance = distance;
                closestEnemy = enemy;
            }
        }

        // Return its transform
        target = closestEnemy.transform;
    }

    private void AimWeapon()
    {
        rotator.LookAt(target.transform);
    }

    private void Fire(bool b_shouldFire)
    {
        // If the state did not change, do not apply anything
        if ((b_isFiring && b_shouldFire) || (!b_isFiring && !b_shouldFire)) return;

        b_isFiring = b_shouldFire;
        HandleWeapons(b_shouldFire);
        HandleFiringAnimations(b_shouldFire);
    }

    // Activates and deactivates the particle system given isActive
    private void HandleWeapons(bool isActive)
    {
        for (int i = 0; i < weapionParticleSystems.Length; ++i)
        {
            var emmisionModule = weapionParticleSystems[i].emission;
            emmisionModule.enabled = isActive;
        }
    }

    private void HandleFiringAnimations(bool isFiring)
    {
        animator.SetBool(ANIM_IS_FIRING, isFiring);
    }
}
