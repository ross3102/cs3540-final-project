using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileShooter : ShootEnemies
{
    public float fireSpeed;
    public int damage;

    public GameObject projectile;
    public AudioClip shootSFX;

    Transform cannonTransform;

    override public void Start()
    {
        base.Start();
        cannonTransform = transform.GetChild(1);
    }

    override public void Update()
    {
        if (enemiesInRange.Count > 0)
        {
            GameObject target = enemiesInRange.Peek();
            cannonTransform.LookAt(target.transform.position + Vector3.up * 1.5f);
        }
        base.Update();
    }

    override public void ShootTarget()
    {
        AudioSource.PlayClipAtPoint(shootSFX, transform.position);
        GameObject newProjectile = Instantiate(projectile, cannonTransform.position, cannonTransform.rotation);
        newProjectile.GetComponent<ProjectileBehavior>().SetDamage(damage);
        newProjectile.GetComponent<Rigidbody>().AddForce(cannonTransform.forward * fireSpeed, ForceMode.VelocityChange);
    }

    override public void UpgradeDamage(int newDamage)
    {
        damage = newDamage;
    }
}
