using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public int startingMoney = 10;
    public Text moneyText;

    static int money = 0;

    // Start is called before the first frame update
    void Start()
    {
        money += startingMoney;
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
