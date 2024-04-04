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
    MeleeDamage meleeDamage;
    bool punching;

    private void Start()
    {
        animator = GetComponent<Animator>();
        meleeDamage = GetComponentInChildren<MeleeDamage>();
        punching = false;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !punching)
        {
            if (meleeDamage.TryInteract()) return;

            punching = true;
            animator.SetInteger("WeaponType_int", 12);

            Invoke(nameof(RegisterHit), 0.175f);
            Invoke(nameof(StopSwinging), 0.5f);
        }
    }

    private void RegisterHit()
    {
        if (meleeDamage.Attack(damage))
        {
            AudioSource.PlayClipAtPoint(punchHitSFX, Camera.main.transform.position);
        }
        else
        {
            AudioSource.PlayClipAtPoint(punchMissSFX, Camera.main.transform.position);
        }
    }

    void StopSwinging()
    {
        animator.SetInteger("WeaponType_int", 0);
        punching = false;
    }
}
