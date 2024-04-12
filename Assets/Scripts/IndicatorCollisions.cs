using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IndicatorCollisions : MonoBehaviour
{
    List<GameObject> floorsInRange = new();
    List<GameObject> objectsInRange = new();

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            floorsInRange.Add(other.gameObject);
        }
        else {
            objectsInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            floorsInRange.Remove(other.gameObject);
        }
        else {
            objectsInRange.Remove(other.gameObject);
        }
    }

    public bool CanPlace()
    {
        return floorsInRange.Count > 0 && objectsInRange.Count == 0;
    }

    public void RemoveAll()
    {
        floorsInRange.Clear();
        objectsInRange.Clear();
    }
}
