using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaceTower : MonoBehaviour
{
    public GameObject[] towerPrefabs;
    public Sprite[] towerImages;
    public GameObject indicatorPrefab;
    public GameObject radiusPreviewPrefab;
    public GameObject towerOptionPrefab;
    public int baseTowerCost = 10;
    public AudioClip placeSound;

    public Color validColor, invalidColor;
    public GameObject towerSelectPanel;

    GameObject indicator;
    Vector3 indicatorPos;
    GameObject radiusPreview;
    MoneyManager money;
    LevelManager levelManager;

    bool indicatorValid;

    int towerIndex = 0;

    int towerCost;

    IndicatorCollisions indicatorCollisions;
    
    void Start()
    {
        indicator = new GameObject("Indicator");
        var indicatorSquare = Instantiate(indicatorPrefab, indicator.transform.position, Quaternion.identity);
        indicatorSquare.transform.SetParent(indicator.transform);
        indicatorCollisions = indicatorSquare.GetComponent<IndicatorCollisions>();
        var towerDiameter = towerPrefabs[towerIndex].GetComponent<ShootEnemies>().radius * 2;
        radiusPreviewPrefab.transform.localScale = new Vector3(towerDiameter, 1, towerDiameter);
        radiusPreview = Instantiate(radiusPreviewPrefab, indicator.transform.position, Quaternion.identity);
        radiusPreview.transform.SetParent(indicator.transform);
        money = GameObject.FindGameObjectWithTag("Player").GetComponent<MoneyManager>();
        levelManager = FindObjectOfType<LevelManager>();
        towerCost = baseTowerCost;


        for (int i = 0; i < towerImages.Length; i++)
        {
            Sprite towerImg = towerImages[i];
            GameObject towerOption = Instantiate(towerOptionPrefab, towerSelectPanel.transform);
            towerOption.transform.SetParent(towerSelectPanel.transform, false);
            towerOption.GetComponent<Image>().sprite = towerImg;
            towerOption.transform.Find("Key").GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
            if (i == 0)
            {
                towerOption.transform.Find("Key").gameObject.SetActive(false);
            }
            towerOption.transform.Find("CostText").GetComponent<TextMeshProUGUI>().text = "$" + (baseTowerCost * (i + 1)).ToString();
        }
        towerSelectPanel.transform.GetChild(towerIndex).localScale = Vector3.one * 1.25f;
    }

    void Update()
    {
        KeyCode towerKey = KeyCode.Alpha1;
        for (int i = 0; i < towerPrefabs.Length; i++)
        {
            if (Input.GetKeyDown(towerKey))
            {
                towerSelectPanel.transform.GetChild(towerIndex).localScale = Vector3.one;
                towerSelectPanel.transform.GetChild(towerIndex).Find("Key").gameObject.SetActive(true);
                towerIndex = i;
                towerSelectPanel.transform.GetChild(towerIndex).localScale = Vector3.one * 1.25f;
                towerSelectPanel.transform.GetChild(towerIndex).Find("Key").gameObject.SetActive(false);
                towerCost = baseTowerCost * (i + 1);
                var towerDiameter = towerPrefabs[towerIndex].GetComponent<ShootEnemies>().radius * 2;
                radiusPreview.transform.localScale = new Vector3(towerDiameter, 1, towerDiameter);
            }
            towerKey++;
        }
        
        if (levelManager.IsPlaceTowerDisabled() || (LevelManager.currentPhase != LevelManager.GamePhase.TowerPlacement && LevelManager.currentPhase != LevelManager.GamePhase.EnemyWave)) {
            indicatorCollisions.RemoveAll();
            indicator.SetActive(false);
            return;
        }

        indicator.SetActive(true);

        if (Input.GetButtonDown("Fire2") && money.HasAtLeast(towerCost))
        {
            if (indicatorValid)
            {
                Instantiate(towerPrefabs[towerIndex], indicatorPos + new Vector3(0, 2, 0), Quaternion.identity);
                money.SpendMoney(towerCost);
                AudioSource.PlayClipAtPoint(placeSound, Camera.main.transform.position);   
            }
        }
        else
        {
            var lookDirection = transform.forward;
            var coef = transform.position.y / lookDirection.y;
            var lookPoint = transform.position - lookDirection * coef;

            indicatorPos = lookPoint;

            indicatorValid = CanPlaceTower();
        }

        indicator.transform.position = indicatorPos;
        indicator.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = indicatorValid ? validColor : invalidColor;
    }

    bool CanPlaceTower()
    {
        if (!money.HasAtLeast(towerCost))
        {
            return false;
        }
        if (!indicatorCollisions.CanPlace()) {
            return false;
        }

        return true;
    }

    public int GetCurrentTowerCost()
    {
        return towerCost;
    }
}
