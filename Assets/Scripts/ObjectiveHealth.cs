using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveHealth : MonoBehaviour
{
    public Slider healthBar;
    public int maxHealth = 5;
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.value = currentHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;

        if (currentHealth <= 0) {
            currentHealth = 0;
            FindObjectOfType<LevelManager>().LevelLost();
        }
        healthBar.value = currentHealth;
    }
}
