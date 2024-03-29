using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradableTower : MonoBehaviour
{
    public enum UpgradeType
    {
        FireRate,
        Damage,
        Radius
    }

    [Serializable]
    public class KeyValuePair<TKey, TValue>
    {
        public TKey key;
        public TValue value;
    }

    [Serializable]
    public class Upgrade
    {
        public int cost;
        public float value;

        public Upgrade(int cost, float value)
        {
            this.cost = cost;
            this.value = value;
        }

        public int intValue => (int)value;
    }

    public AudioClip upgradeSound;
    public List<KeyValuePair<UpgradeType, Upgrade[]>> upgradeValueList = new()
    {
        new() {
            key = UpgradeType.FireRate,
            value = new Upgrade[]
            {
                new(0, 1.0f), // Initial
                new(10, 0.9f),
                new(15, 0.8f),
                new(20, 0.7f),
            }
        },
        new() {
            key = UpgradeType.Damage,
            value = new Upgrade[]
            {
                new(0, 1), // Initial
                new(20, 2),
                new(30, 3),
                new(50, 4)
            }
        },
        new() {
            key = UpgradeType.Radius,
            value = new Upgrade[]
            {
                new(0, 8), // Initial
                new(10, 10)
            }
        }
    };

    SortedDictionary<UpgradeType, Upgrade[]> upgradeValues = new();

    public GameObject upgradePanelPrefab;
    
    SortedDictionary<UpgradeType, int> upgradeLevels = new();

    GameObject upgradesPanel;
    ShootEnemies shootEnemies;
    MoneyManager money;

    bool canUpgrade = false;

    void Start()
    {
        upgradesPanel = FindObjectOfType<LevelManager>().upgradesPanel;
        shootEnemies = GetComponent<ShootEnemies>();
        money = GameObject.FindGameObjectWithTag("Player").GetComponent<MoneyManager>();
        canUpgrade = false;
        foreach (KeyValuePair<UpgradeType, Upgrade[]> upgrade in upgradeValueList)
        {
            upgradeValues[upgrade.key] = upgrade.value;
            upgradeLevels[upgrade.key] = 0;
            DoUpgrade(upgrade.key, 0);
        }
    }

    void Update()
    {
        if (canUpgrade)
        {
            if (!upgradesPanel.activeSelf)
            {
                SetupMenu();
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                DoUpgrade(UpgradeType.FireRate, upgradeLevels[UpgradeType.FireRate] + 1);
                SetupMenu();
            }
            if (Input.GetKeyDown(KeyCode.X) && money.HasAtLeast(10))
            {
                DoUpgrade(UpgradeType.Damage, upgradeLevels[UpgradeType.Damage] + 1);
                SetupMenu();
            }
            if (Input.GetKeyDown(KeyCode.C) && money.HasAtLeast(10))
            {
                DoUpgrade(UpgradeType.Radius, upgradeLevels[UpgradeType.Radius] + 1);
                SetupMenu();
            }
        }
        else
        {
            upgradesPanel.SetActive(false);
        }
    }

    void SetupMenu()
    {
        upgradesPanel.SetActive(true);
        var upgrades = upgradesPanel.transform.Find("Upgrades");
        foreach (Transform child in upgrades)
        {
            Destroy(child.gameObject);
        }

        int i = 0;

        // iterate over upgradeLevels keys
        foreach (UpgradeType upgradeType in upgradeLevels.Keys)
        {
            GameObject panel = MakePanel(upgradeType);
            if (panel != null)
            {
                panel.transform.SetParent(upgrades.transform);
                panel.transform.localPosition = new Vector3(0, -225*i, 0);
                i++;
            }
        }
    }

    GameObject MakePanel(UpgradeType upgradeType)
    {
        int curLevel = upgradeLevels[upgradeType];
        Upgrade[] upgrades = upgradeValues[upgradeType];
        if (curLevel >= upgrades.Length - 1)
        {
            return null;
        }

        GameObject panel = Instantiate(upgradePanelPrefab, upgradesPanel.transform);
        panel.transform.Find("UpgradeTitle").GetComponent<TextMeshProUGUI>().text =
            UpgradeTitle(upgradeType);
        panel.transform.Find("UpgradeCost").GetComponent<TextMeshProUGUI>().text =
            "$" + upgrades[curLevel + 1].cost.ToString();
        panel.transform.Find("Equation").Find("OldValue").GetComponent<TextMeshProUGUI>().text =
            DisplayValue(upgradeType, curLevel);
        panel.transform.Find("Equation").Find("NewValue").GetComponent<TextMeshProUGUI>().text =
            DisplayValue(upgradeType, curLevel + 1);
        panel.name = upgradeType.ToString() + "Panel";
        return panel;
    }

    string UpgradeTitle(UpgradeType upgradeType)
    {
        return upgradeType switch
        {
            UpgradeType.FireRate => "Fire Rate",
            UpgradeType.Damage => "Damage",
            UpgradeType.Radius => "Radius",
            _ => "",
        };
    }

    string DisplayValue(UpgradeType upgradeType, int level)
    {
        Upgrade[] upgrades = upgradeValues[upgradeType];
        if (level >= upgrades.Length)
        {
            return "";
        }

        return upgradeType switch
        {
            UpgradeType.Damage or UpgradeType.Radius => upgrades[level].intValue.ToString(),
            _ => upgrades[level].value.ToString("0.00"),
        };
    }

    void DoUpgrade(UpgradeType upgradeType, int level)
    {
        Upgrade[] upgrades = upgradeValues[upgradeType];
        if (level >= upgrades.Length || !money.HasAtLeast(upgrades[level].cost))
        {
            return;
        }

        AudioSource.PlayClipAtPoint(upgradeSound, transform.position);
        money.AddMoney(-upgrades[level].cost);
        switch (upgradeType)
        {
            case UpgradeType.FireRate:
                shootEnemies.fireInterval = upgrades[level].value;
                break;
            case UpgradeType.Damage:
                shootEnemies.damage = upgrades[level].intValue;
                break;
            case UpgradeType.Radius:
                shootEnemies.radius = upgrades[level].intValue;
                break;
        }
        upgradeLevels[upgradeType] = level;
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
