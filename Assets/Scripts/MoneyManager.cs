using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public Text moneyText;
    int money;

    // Start is called before the first frame update
    void Start()
    {
        money = 0;
        SetMoneyText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetMoneyAmount()
    {
        return money;
    }

    public void AddMoney(int amount)
    {
        money += amount;
        SetMoneyText();
    }

    public void SpendMoney(int amount)
    {
        money -= amount;
        SetMoneyText();
    }

    public bool HasAtLeast(int amount)
    {
        return money >= amount;
    }

    void SetMoneyText()
    {
        moneyText.text = "$" + money.ToString();
    }
}
