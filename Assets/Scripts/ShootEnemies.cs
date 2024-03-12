using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootEnemies : MonoBehaviour
{
    public float radius;
    public float fireInterval;
    public float fireSpeed;
    public int damage;

    public GameObject projectile;

    Queue<GameObject> enemiesInRange;
    float lastShot;
    Transform cannonTransform;

    // Start is called before the first frame update
    void Start()
    {
        enemiesInRange = new Queue<GameObject>();
        lastShot = Time.time;
        GetComponent<CapsuleCollider>().radius = radius;
        cannonTransform = transform.GetChild(1);
    }

    void Update()
    {
        while (enemiesInRange.Count > 0 && TargetGone(enemiesInRange.Peek()))
        {
            enemiesInRange.Dequeue();
        }

        if (enemiesInRange.Count > 0)
        {
            GameObject target = enemiesInRange.Peek();
            cannonTransform.LookAt(target.transform.position + Vector3.up * 1.5f);
            if (Time.time - lastShot > fireInterval)
            {
                ShootTarget();
            }
        }
        else
        {
            cannonTransform.localRotation = Quaternion.Euler(-90, 0, 0);
        }
    }

    bool TargetGone(GameObject target)
    {
        return target.IsDestroyed() || target.GetComponent<EnemyBehavior>().GetHealth() <= 0;
    }

    void ShootTarget()
    {
        GameObject newProjectile = Instantiate(projectile, cannonTransform.position, cannonTransform.rotation);
        newProjectile.GetComponent<ProjectileBehavior>().SetDamage(damage);
        newProjectile.GetComponent<Rigidbody>().AddForce(cannonTransform.forward * fireSpeed, ForceMode.VelocityChange);
        lastShot = Time.time;
    }

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
}
