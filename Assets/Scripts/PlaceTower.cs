using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    public GameObject towerPrefab;
    public GameObject indicatorPrefab;
    public GameObject radiusPreviewPrefab;
    public int towerCost = 10;
    public AudioClip placeSound;

    public Color validColor, invalidColor;

    GameObject indicator;
    Vector3 indicatorPos;
    MoneyManager money;
    LevelManager levelManager;

    bool indicatorValid;
    
    void Start()
    {
        indicator = new GameObject("Indicator");
        var indicatorSquare = Instantiate(indicatorPrefab, indicator.transform.position, Quaternion.identity);
        indicatorSquare.transform.SetParent(indicator.transform);
        var towerDiameter = towerPrefab.GetComponent<ShootEnemies>().radius * 2;
        radiusPreviewPrefab.transform.localScale = new Vector3(towerDiameter, 1, towerDiameter);
        var radiusPreview = Instantiate(radiusPreviewPrefab, indicator.transform.position, Quaternion.identity);
        radiusPreview.transform.SetParent(indicator.transform);
        money = GameObject.FindGameObjectWithTag("Player").GetComponent<MoneyManager>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        if (levelManager.PlaceTowerDisabled() || (LevelManager.currentPhase != LevelManager.GamePhase.TowerPlacement && LevelManager.currentPhase != LevelManager.GamePhase.EnemyWave)) {
            indicator.SetActive(false);
            return;
        }

        indicator.SetActive(true);

        if (Input.GetButtonDown("Fire2") && money.HasAtLeast(towerCost))
        {
            if (indicatorValid)
            {
                Instantiate(towerPrefab, indicatorPos + new Vector3(0, 2, 0), Quaternion.identity);
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
