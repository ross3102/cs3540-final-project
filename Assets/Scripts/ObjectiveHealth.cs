using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveHealth : MonoBehaviour
{
    public int maxHealth = 5;

    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;

        if (currentHealth <= 0) {
            FindObjectOfType<LevelManager>().LevelLost();
        }
    }
}
