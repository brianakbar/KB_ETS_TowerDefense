using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text balanceText; 

    Bank bank;
    // Start is called before the first frame update
    void Awake()
    {
        bank = FindObjectOfType<Bank>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bank == null) { return; }
        balanceText.text = "Gold: " + bank.CurrentBalance;
    }
}
