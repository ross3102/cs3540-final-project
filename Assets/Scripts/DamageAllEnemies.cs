using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAllEnemies : ShootEnemies
{
    public int damage;
    public AudioClip shootSFX;

    public override void ShootTarget()
    {
        AudioSource.PlayClipAtPoint(shootSFX, transform.position);
        foreach (GameObject enemy in enemiesInRange)
        {
            enemy.GetComponent<EnemyBehavior>().TakeDamage(damage);
        }
    }

    public override void UpgradeDamage(int newDamage)
    {
        damage = newDamage;
    }
}
