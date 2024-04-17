using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public int maxHealth = 10;
    public GameObject lootPrefab;
    public AudioClip deathSFX;

    public int minDropVal;
    public int maxDropVal;
    public int dropRate; // Higher = higher chance of dropping

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

        PickupBehavior pickup = lootPrefab.GetComponent<PickupBehavior>();
        pickup.minVal = minDropVal;
        pickup.maxVal = maxDropVal;

        Invoke("DestroyEnemy", 2.5f);
        FindObjectOfType<LevelManager>().EnemyDestroyed();
        AudioSource.PlayClipAtPoint(deathSFX, transform.position);
    }

    void DestroyEnemy()
    {
        int dropVal = Random.Range(0, dropRate);

        if (dropVal > 3)
        {
            Instantiate(lootPrefab, transform.position, transform.rotation);
        }

        gameObject.SetActive(false);

        Destroy(gameObject, 0.5f);
    }
}
