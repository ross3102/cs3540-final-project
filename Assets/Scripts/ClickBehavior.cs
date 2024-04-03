using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickBehavior : MonoBehaviour
{
    public int damage;
    public float hitRange = 10f;

    public AudioClip punchHitSFX;
    public AudioClip punchMissSFX;

    Animator animator;
    bool canSwing;

    private void Start()
    {
        animator = GetComponent<Animator>();
        canSwing = true;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && canSwing)
        {
            RaycastHit raycastHit;

            if (Physics.Raycast(transform.position, transform.forward, out raycastHit, 5.5f))
            {
                if (raycastHit.transform != null)
                {
                    var clicked = raycastHit.transform.gameObject;
                    if (clicked.CompareTag("NPC"))
                    {
                        NPCText text = clicked.GetComponentInChildren<NPCText>();
                        text.CycleText();
                        return;
                    }
                }
            }

            AudioSource.PlayClipAtPoint(punchHitSFX, Camera.main.transform.position);
            canSwing = false;
            animator.SetFloat("Speed_f", 0f);
            animator.SetInteger("WeaponType_int", 12);

            Invoke("RegisterHit", 0.5f);
            Invoke("StopSwinging", 1f);
            
        }
    }

    private void RegisterHit()
    {
        RaycastHit raycastHit;

        if (Physics.Raycast(transform.position, transform.forward, out raycastHit, hitRange))
        {
            if (raycastHit.transform != null)
            {
                var clicked = raycastHit.transform.gameObject;
                if (clicked.CompareTag("Enemy"))
                {
                    clicked.GetComponent<EnemyBehavior>().TakeDamage(damage);
                } 
            }
        }
    }

    void StopSwinging()
    {
        animator.SetInteger("WeaponType_int", 0);
        canSwing = true;
    }
}
