using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradableTower : MonoBehaviour
{
    public AudioClip upgradeSound;

    GameObject upgradePanel;
    ShootEnemies shootEnemies;
    MoneyManager money;

    bool canUpgrade = false;

    void Start()
    {
        upgradePanel = FindObjectOfType<LevelManager>().upgradePanel;
        shootEnemies = GetComponent<ShootEnemies>();
        money = GameObject.FindGameObjectWithTag("Player").GetComponent<MoneyManager>();
        canUpgrade = false;
    }

    void Update()
    {
        if (canUpgrade)
        {
            if (!upgradePanel.activeSelf)
            {
                SetupMenu();
            }
            if (Input.GetKeyDown(KeyCode.Z) && money.HasAtLeast(10))
            {
                money.AddMoney(-10);
                shootEnemies.fireInterval *= 0.9f;
                AudioSource.PlayClipAtPoint(upgradeSound, transform.position);
                SetupMenu();
            }
            if (Input.GetKeyDown(KeyCode.X) && money.HasAtLeast(10))
            {
                money.AddMoney(-10);
                shootEnemies.damage += 1;
                AudioSource.PlayClipAtPoint(upgradeSound, transform.position);
                SetupMenu();
            }
            if (Input.GetKeyDown(KeyCode.C) && money.HasAtLeast(10))
            {
                money.AddMoney(-10);
                shootEnemies.radius += 2;
                AudioSource.PlayClipAtPoint(upgradeSound, transform.position);
                SetupMenu();
            }
        }
        else
        {
            upgradePanel.SetActive(false);
        }
    }

    void SetupMenu()
    {
        upgradePanel.SetActive(true);
        var frPanel = upgradePanel.transform.Find("FireRatePanel");
        frPanel.Find("Equation").Find("OldValue").GetComponent<TextMeshProUGUI>().text = shootEnemies.fireInterval.ToString("0.00");
        frPanel.Find("Equation").Find("NewValue").GetComponent<TextMeshProUGUI>().text = (shootEnemies.fireInterval * 0.9f).ToString("0.00");
        var dmgPanel = upgradePanel.transform.Find("DamagePanel");
        dmgPanel.Find("Equation").Find("OldValue").GetComponent<TextMeshProUGUI>().text = shootEnemies.damage.ToString();
        dmgPanel.Find("Equation").Find("NewValue").GetComponent<TextMeshProUGUI>().text = (shootEnemies.damage + 1).ToString();
        var radiusPanel = upgradePanel.transform.Find("RadiusPanel");
        radiusPanel.Find("Equation").Find("OldValue").GetComponent<TextMeshProUGUI>().text = shootEnemies.radius.ToString();
        radiusPanel.Find("Equation").Find("NewValue").GetComponent<TextMeshProUGUI>().text = (shootEnemies.radius + 2).ToString();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canUpgrade = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canUpgrade = false;
        }
    }
}
