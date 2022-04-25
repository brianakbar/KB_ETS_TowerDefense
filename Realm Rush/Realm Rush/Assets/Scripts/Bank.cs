using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150;
    [SerializeField] int currentBalance;
    [SerializeField] GameObject gameOverCanvas;
    public int CurrentBalance { get { return currentBalance; } }

    void Awake() {
        currentBalance = startingBalance;
    }

    void Start() {
        Time.timeScale = 1f;
    }

    public void Deposit(int amount) {
        currentBalance += Mathf.Abs(amount);
    }

    public void Withdraw(int amount) {
        currentBalance -= Mathf.Abs(amount);

        if(currentBalance < 0) {
            Time.timeScale = 0f;
            gameOverCanvas.SetActive(true);
        }
    }
}
