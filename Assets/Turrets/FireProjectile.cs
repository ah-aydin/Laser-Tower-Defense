using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetLocator))]
public class FireProjectile : MonoBehaviour
{
    [Header("Damage stats")]
    [Tooltip("Fire rate in rounds per second")]
    [SerializeField] [Range(0.00001f, 50f)] float RoundsPerSecond = 10f;
    [SerializeField] float damagePerShot = 1;
    [SerializeField] bool isSplashDamage = false;
    [SerializeField] float splashDamage = 1;
    [SerializeField] [Range(0.1f, 30)] float splashRadius = 5f;

    [Header("Turret components")]
    [SerializeField] Transform[] bulletSpawns;
    [SerializeField] List<ParticleSystem> firingParticles;

    [Header("Projectile prefab")]
    [SerializeField] private GameObject projectilePrefab;

    // Animation
    private Animator animator;

    private TargetLocator targetLocator;

    private float delayBetweenShots;
    private float timeSinceLastFire;
    private bool b_isFiring;            // Keep's track of the current state of firing

    private void Start()
    {
        animator = GetComponent<Animator>();
        targetLocator = GetComponent<TargetLocator>();

        // Calculate delay between shots
        delayBetweenShots = 1f / RoundsPerSecond;
        timeSinceLastFire = 0;

        b_isFiring = true;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if (timeSinceLastFire >= delayBetweenShots)
        {
            this.Fire();
            timeSinceLastFire -= delayBetweenShots;
        }
        timeSinceLastFire += Time.deltaTime;
    }

    private void Fire()
    {
        if (!targetLocator.Target) return;

        SpawnProjectile();
    }

    private void SpawnProjectile()
    {
        // Chhose the projectile spawn locaiton
        int spanwerIndex = Random.Range(0, bulletSpawns.Length);
        Transform spawnLocation = bulletSpawns[spanwerIndex];

        // Spawn projectile and set it's stats
        Projectile projectile = Instantiate(projectilePrefab, spawnLocation.position, spawnLocation.rotation).GetComponent<Projectile>();
        projectile.SetStats(damagePerShot, splashDamage, splashRadius, isSplashDamage);
    }
}
