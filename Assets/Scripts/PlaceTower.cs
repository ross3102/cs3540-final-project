using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    public GameObject towerPrefab;
    public GameObject indicatorPrefab;

    GameObject indicator;
    Vector3 indicatorPos;
    
    // Start is called before the first frame update
    void Start()
    {
        indicator = Instantiate(indicatorPrefab);
        indicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        indicator.SetActive(true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                indicatorPos = ClosestGrid(hit.point);
                indicator.transform.position = ClosestGrid(hit.point);
            }
            
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                Instantiate(towerPrefab, indicatorPos + new Vector3(0, 2, 0), Quaternion.identity);
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
