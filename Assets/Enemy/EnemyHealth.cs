using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int startingHealth = 100;
    private int currentHealth = 0;

    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        currentHealth = startingHealth;
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    void ProcessHit()
    {
        currentHealth -= 1;
        if (currentHealth <= 0)
        {
            // TODO add more specticle
            gameObject.SetActive(false);
            enemy.BankReward();
        }
    }
}
