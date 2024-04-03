using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    public GameObject[] towerPrefabs;
    public GameObject indicatorPrefab;
    public GameObject radiusPreviewPrefab;
    public int baseTowerCost = 10;
    public AudioClip placeSound;

    public Color validColor, invalidColor;

    GameObject indicator;
    Vector3 indicatorPos;
    GameObject radiusPreview;
    MoneyManager money;

    bool indicatorValid;

    int towerIndex = 0;

    int towerCost;
    
    void Start()
    {
        indicator = new GameObject("Indicator");
        var indicatorSquare = Instantiate(indicatorPrefab, indicator.transform.position, Quaternion.identity);
        indicatorSquare.transform.SetParent(indicator.transform);
        var towerDiameter = towerPrefabs[towerIndex].GetComponent<ShootEnemies>().radius * 2;
        radiusPreviewPrefab.transform.localScale = new Vector3(towerDiameter, 1, towerDiameter);
        radiusPreview = Instantiate(radiusPreviewPrefab, indicator.transform.position, Quaternion.identity);
        radiusPreview.transform.SetParent(indicator.transform);
        money = GameObject.FindGameObjectWithTag("Player").GetComponent<MoneyManager>();
        towerCost = baseTowerCost;
    }

    void Update()
    {
        if (LevelManager.currentPhase != LevelManager.GamePhase.TowerPlacement && LevelManager.currentPhase != LevelManager.GamePhase.EnemyWave) {
            indicator.SetActive(false);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            towerIndex = 0;
            towerCost = baseTowerCost;
            var towerDiameter = towerPrefabs[towerIndex].GetComponent<ShootEnemies>().radius * 2;
            radiusPreview.transform.localScale = new Vector3(towerDiameter, 1, towerDiameter);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            towerIndex = 1;
            towerCost = baseTowerCost * 2;
            var towerDiameter = towerPrefabs[towerIndex].GetComponent<ShootEnemies>().radius * 2;
            radiusPreview.transform.localScale = new Vector3(towerDiameter, 1, towerDiameter);
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
        indicator.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = indicatorValid ? Color.green : Color.red;
    }

    bool CanPlaceTower()
    {
        RaycastHit hit;
        if (!money.HasAtLeast(towerCost))
        {
            return false;
        }
        if (!Physics.Raycast(transform.position, transform.forward, out hit, 50f))
        {
            return false;
        }
        if (!hit.collider.CompareTag("Floor"))
        {
            return false;
        }

        return true;
    }

    public int GetCurrentTowerCost()
    {
        return towerCost;
    }
}
