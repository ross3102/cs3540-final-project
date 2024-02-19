using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDamage : MonoBehaviour
{
    public int damage;

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
            {
                if (raycastHit.transform != null)
                {
                   var clicked = raycastHit.transform.gameObject;
                   if (clicked.CompareTag("Enemy")) {
                       clicked.GetComponent<EnemyBehavior>().TakeDamage(damage);
                   }
                }
            }
        }
    }
}
