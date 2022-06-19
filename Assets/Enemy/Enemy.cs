using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int bankReward = 25;
    [SerializeField] int bankPenalty = 25;

    Bank bank;

    private void Start()
    {
        bank = FindObjectOfType<Bank>();
    }

    public void BankReward()
    {
        if (!bank) return;
        bank.Deposit(bankReward);
    }

    public void BankPenalty()
    {
        if (!bank) return;
        bank.Withdraw(bankPenalty);
    }
}
