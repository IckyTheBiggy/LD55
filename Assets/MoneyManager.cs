using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private int _startingMoney;
    [HideInInspector] public int Money;
    
    private void Start()
    {
        Money = _startingMoney;
        UpdateMoneyText();
    }

    public void UpdateMoneyText() =>  _moneyText.text = Money.ToString();

    public void SubtractMoney(int value)
    {
        Money -= value;
        UpdateMoneyText();
    }

    public void AddMoney(int value)
    {
        Money += value;
        UpdateMoneyText();
    }
}
