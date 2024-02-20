using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDamage : MonoBehaviour
{
    public int damage;
    public float hitRange = 5f;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            RaycastHit raycastHit;
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(transform.position, transform.forward, out raycastHit, hitRange))
            {
                if (raycastHit.transform != null)
                {
                   var clicked = raycastHit.transform.gameObject;
                   if (clicked.CompareTag("Enemy")) {
                       clicked.GetComponent<EnemyBehavior>().TakeDamage(damage);
                   }
                }
            }
            animator.SetInteger("WeaponType_int", 12);
            Invoke("StopSwinging", 1);
        }
    }

    void StopSwinging()
    {
        animator.SetInteger("WeaponType_int", 0);
    }
}
