using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public int maxHealth = 10;
    public AudioClip deathSFX;

    Animator animator;
    FollowPath followPath;

    int health;

    void Start()
    {
        health = maxHealth;

        followPath = GetComponent<FollowPath>();
        animator = GetComponent<Animator>();

        animator.SetInteger("WeaponType_int", 0);
        animator.SetFloat("Speed_f", 0.3f);
    }

    public int GetHealth()
    {
        return health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy took " + damage + " damage, health: " + health);
        if (health <= 0)
        {
            animator.SetTrigger("Death_b");
            followPath.SetSpeed(0);
            GetComponent<Collider>().enabled = false;
            AudioSource.PlayClipAtPoint(deathSFX, transform.position);
            Destroy(gameObject, 2.5f);
        }
    }
}
