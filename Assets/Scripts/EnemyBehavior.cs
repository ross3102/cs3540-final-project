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
    HealthBar healthBar;

    int health;

    void Start()
    {
        health = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>();

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
        healthBar.UpdateHealth(health, maxHealth);
        Debug.Log("Enemy took " + damage + " damage, health: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Death_b");
        followPath.SetSpeed(0);
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 2.5f);
        FindObjectOfType<LevelManager>().EnemyDestroyed();
        AudioSource.PlayClipAtPoint(deathSFX, transform.position);
    }
}
