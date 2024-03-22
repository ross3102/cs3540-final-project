using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    public GameObject towerPrefab;
    public GameObject indicatorPrefab;
    public int towerCost = 10;
    public AudioClip placeSound;

    GameObject indicator;
    Vector3 indicatorPos;
    MoneyManager money;
    
    // Start is called before the first frame update
    void Start()
    {
        indicator = Instantiate(indicatorPrefab);
        indicator.SetActive(false);
        money = GameObject.FindGameObjectWithTag("Player").GetComponent<MoneyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.currentPhase != LevelManager.GamePhase.TowerPlacement) return;

        if (Input.GetButtonDown("Fire2") && money.HasAtLeast(towerCost))
        {
            indicator.SetActive(true);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
            {
                Instantiate(towerPrefab, indicatorPos + new Vector3(0, 2, 0), Quaternion.identity);
                money.SpendMoney(towerCost);
                AudioSource.PlayClipAtPoint(placeSound, Camera.main.transform.position);   
            }
        }
        else
        {
            indicator.SetActive(true);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
            {
                if (hit.collider.CompareTag("Floor"))
                {
                    indicatorPos = hit.point;
                    indicator.transform.position = hit.point;
                }
            }
        }

    }

    private void FixedUpdate()
    {
        
    }
}
