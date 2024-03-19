using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    public GameObject towerPrefab;
    public GameObject indicatorPrefab;
    public AudioClip placeSound;

    GameObject indicator;
    Vector3 indicatorPos;
    bool isValidPlacement;
    Vector3 recentPlacement;
    
    // Start is called before the first frame update
    void Start()
    {
        indicator = Instantiate(indicatorPrefab);
        indicator.SetActive(false);
        isValidPlacement = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (LevelManager.currentPhase != LevelManager.GamePhase.TowerPlacement) return;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                indicator.SetActive(true);
                indicatorPos = ClosestGrid(hit.point);
                indicator.transform.position = ClosestGrid(hit.point);
                isValidPlacement = true;
            }
            
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (isValidPlacement && recentPlacement != indicatorPos)
            {
                recentPlacement = indicatorPos;
                Instantiate(towerPrefab, indicatorPos + new Vector3(0, 2, 0), Quaternion.identity);
                AudioSource.PlayClipAtPoint(placeSound, Camera.main.transform.position);   
            }
        }

    }

    private Vector3 ClosestGrid (Vector3 coord)
    {
        return new Vector3(Mathf.Round(coord.x), Mathf.Round(coord.y), Mathf.Round(coord.z));
    }

    private void FixedUpdate()
    {
        
    }
}
