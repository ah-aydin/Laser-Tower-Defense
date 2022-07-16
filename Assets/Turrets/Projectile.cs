using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 100000)] private float travelSpeed = 10f;
    [Tooltip("How much time the projectile will travel before getting despawned")]
    [SerializeField] [Range(0.1f, 10f)] private float timeToLive = 2f;
    [SerializeField] private GameObject explosionParticle;

    [Tooltip("Put the enemy layer mask here")]
    [SerializeField] LayerMask enemyMask;

    // These values are set by the turret that fires the object
    private float directHitDamage;
    private float splashDamage;
    private float splashRadius;
    private bool b_isSplashDamage;

    private Rigidbody rb;

    private string ENEMY_TAG = "Enemy";

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * travelSpeed;

        StartCoroutine(DestroyProjectile());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ENEMY_TAG)
        {
            EnemyHealth enemyHeatlh = other.GetComponent<EnemyHealth>();
            if (enemyHeatlh)
            {
                enemyHeatlh.ProcessHit(directHitDamage);
            }
        }
        if (explosionParticle)
        {
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
        }
        DealSplashDamage();
        Destroy(gameObject);
    }

    private void DealSplashDamage()
    {
        // If it is not a splash damage projectile, ignore the method
        if (!b_isSplashDamage) return;

        // Get objects within the radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, splashRadius, enemyMask);

        foreach (Collider collider in colliders)
        {
            // If it is not an enemy, ignore
            if (collider.tag != ENEMY_TAG) continue;

            EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
            if (! enemyHealth) continue;

            // Calculate damage falloff
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            float damage = splashDamage * (1 - distance / splashDamage);

            // Deal damage
            enemyHealth.ProcessHit(damage);
        }
    }

    public void SetStats(float directHitDamage, float splashDamage, float splashRadius, bool isSplashDamage)
    {
        this.directHitDamage = Mathf.Abs(directHitDamage);
        this.splashDamage = Mathf.Abs(splashDamage);
        this.splashRadius = Mathf.Abs(splashRadius);
        this.b_isSplashDamage = isSplashDamage;
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(timeToLive);
        Destroy(gameObject);
    }
}
