using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    public GameObject towerPrefab;
    public GameObject indicatorPrefab;

    GameObject indicator;
    Vector3 indicatorPos;
    float tiltAngle = 45f;
    
    // Start is called before the first frame update
    void Start()
    {
        indicator = Instantiate(indicatorPrefab);
        indicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            indicator.SetActive(true);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
            {
                Instantiate(towerPrefab, indicatorPos + new Vector3(0, 2, 0), Quaternion.identity);
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
