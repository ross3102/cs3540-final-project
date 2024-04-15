using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ShootEnemies : MonoBehaviour
{
    public float fireInterval;
    public float radius;

    protected Queue<GameObject> enemiesInRange;
    CapsuleCollider radiusTrigger;

    float lastShot;

    public virtual void Start()
    {
      lastShot = Time.time;
      enemiesInRange = new Queue<GameObject>();
      radiusTrigger = GetComponent<CapsuleCollider>();
      radiusTrigger.radius = radius;
    }

    public virtual void Update()
    {
      radiusTrigger.radius = radius;

      while (enemiesInRange.Count > 0 && TargetGone(enemiesInRange.Peek()))
      {
          enemiesInRange.Dequeue();
      }

      if (enemiesInRange.Count > 0) {
        if (Time.time - lastShot > fireInterval)
        {
            ShootTarget();
            lastShot = Time.time;
        }
      }
    }

    bool TargetGone(GameObject target)
    {
        return target.IsDestroyed() || target.GetComponent<EnemyBehavior>().GetHealth() <= 0;
    }

    public abstract void ShootTarget();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Enqueue(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Dequeue();
        }
    }   

    public void UpgradeRadius(float newRadius)
    {
        radius = newRadius;
    }

    public void UpgradeFireInterval(float newFireInterval)
    {
        fireInterval = newFireInterval;
    }

    public abstract void UpgradeDamage(int newDamage);
}
