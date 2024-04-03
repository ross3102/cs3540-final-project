using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehavior : MonoBehaviour
{
    public int minVal = 1;
    public int maxVal = 15;
    public int value;

    // Start is called before the first frame update
    void Start()
    {
        value = Random.Range(minVal, maxVal);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<MoneyManager>().AddMoney(value);
            Destroy(gameObject);
        }
    }
}
