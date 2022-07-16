using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetLocator))]
[RequireComponent(typeof(Animator))]
public class FireHitscan : MonoBehaviour
{
    [Header("Damage stats")]
    [Tooltip("Fire rate in rounds per second")]
    [SerializeField] [Range(0.00001f, 50f)] float RoundsPerSecond = 10f;
    [SerializeField] float damagePerShot = 1;

    [Header("Turret components")]
    [SerializeField] Transform bulletSpawn;
    [SerializeField] List<ParticleSystem> firingParticles;

    // Raycast
    private LayerMask enemyMask;

    // Line renderer
    private LineRenderer lineRenderer;

    // Animation
    private Animator animator;
    private string ANIM_IS_FIRING = "IsFiring";

    // Light
    Light muzzleLight;

    private TargetLocator targetLocator;

    private float delayBetweenShots;
    private float timeSinceLastFire;
    private bool b_isFiring;            // Keep's track of the current state of firing

    private void Start()
    {
        enemyMask = LayerMask.GetMask("Enemy");
        
        lineRenderer = GetComponentInChildren<LineRenderer>();
        animator = GetComponent<Animator>();
        muzzleLight = GetComponentInChildren<Light>();
        targetLocator = GetComponent<TargetLocator>();

        // Calculate delay between shots
        delayBetweenShots = 1f / RoundsPerSecond;
        timeSinceLastFire = 0;

        b_isFiring = true;
        HandleEffects(false);
        RemoveMuzzleEffects();
    }

    private void Update()
    {
        HandleEffects(targetLocator.Target != null);
    }

    private void FixedUpdate()
    {
        if (timeSinceLastFire >= delayBetweenShots)
        {
            Fire();
            timeSinceLastFire -= delayBetweenShots;
        }
        timeSinceLastFire += Time.deltaTime;
    }

    private void Fire()
    {
        // Do not fire if there is no target
        if (!targetLocator.Target) return;

        RaycastCheck();
    }

    private void RaycastCheck()
    {
        // Check to see if there is a hit
        RaycastHit hit;
        if (Physics.Raycast(bulletSpawn.position, bulletSpawn.forward, out hit, targetLocator.Range, enemyMask))
        {
            // If so get the enemYHealth component
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth) // If component exists
            {
                // Deal damage
                enemyHealth.ProcessHit(damagePerShot);
                HandleMuzzleEffects(hit.distance);
            }
        }
        else
        {
            HandleMuzzleEffects(targetLocator.Range);
        }
    }
    
    private void HandleMuzzleEffects(float length)
    {
        lineRenderer.enabled = true;
        muzzleLight.enabled = true;
        Invoke("RemoveMuzzleEffects", delayBetweenShots / 2.5f);
        lineRenderer.SetPosition(1, new Vector3(0, 0, length / 2 + 0.5f));
    }

    private void RemoveMuzzleEffects()
    {
        lineRenderer.enabled = false;
        muzzleLight.enabled = false;
    }

    private void HandleEffects(bool shouldFire)
    {
        if ((b_isFiring && shouldFire) || (!b_isFiring && !shouldFire)) return;

        b_isFiring = shouldFire;

        HandleParticles(shouldFire);
        HandleAnimation(shouldFire);
    }

    private void HandleParticles(bool isEnabled)
    {
        foreach(ParticleSystem ps in firingParticles)
        {
            var emissionModule = ps.emission;
            emissionModule.enabled = isEnabled;
        }
    }

    private void HandleAnimation(bool firing)
    {
        animator.SetBool(ANIM_IS_FIRING, firing);
    }
}
