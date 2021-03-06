using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float startingHealth = 100;
    [Tooltip("Adds to max hitpoints when enemy dies")]
    [SerializeField] float difficultyRamp = 1;

    private float currentHealth = 0;

    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        currentHealth = startingHealth;
    }

    public void ProcessHit(float ammount)
    {
        currentHealth -= Mathf.Abs(ammount);
        if (currentHealth <= 0)
        {
            // TODO add more specticle
            gameObject.SetActive(false);
            startingHealth += difficultyRamp;
            enemy.BankReward();
        }
    }
}
