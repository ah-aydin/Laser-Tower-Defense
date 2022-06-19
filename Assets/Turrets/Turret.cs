using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] int cost = 60;

    public bool CreateTurret(Turret turret, Vector3 position)
    {
        // Get the bank from the scene
        Bank bank = FindObjectOfType<Bank>();
        if (!bank) return false;

        // Check if there is enough money
        if (bank.CurrentBalance < cost) return false;

        // Instantiate object and withdraw cash
        Instantiate(turret.gameObject, position, Quaternion.identity);
        bank.Withdraw(cost);
        return true;
    }
}
