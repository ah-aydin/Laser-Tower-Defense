using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 200;

    private int currentBalance;
    public int CurrentBalance { get { return currentBalance; } }

    TextMeshProUGUI displayBalance;

    private void Awake()
    {
        currentBalance = startingBalance;

        displayBalance = FindObjectOfType<UI_DisplayBalanceText>().gameObject.GetComponent<TextMeshProUGUI>();
        UpdateDisaplay();
    }

    public void Deposit(int ammount)
    {
        currentBalance += Mathf.Abs(ammount);
        UpdateDisaplay();
    }

    public void Withdraw(int ammount)
    {
        currentBalance -= Mathf.Abs(ammount);
        UpdateDisaplay();
        // TODO replace this with an actual health system
        if (currentBalance < 0)
        {
            // Temporary game over state
            ReloadScene();
        }
    }

    void UpdateDisaplay()
    {
        displayBalance.text = $"Cash: ${currentBalance}";
    }

    void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex); 
    }
}
