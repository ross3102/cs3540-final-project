using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTowerBehavior : MonoBehaviour
{
    List<GameObject> enemiesInRange;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        enemiesInRange = new();
        anim = GetComponent<Animator>();
        anim.SetBool("active", false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemiesInRange.Add(other.gameObject);
            anim.SetBool("active", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemiesInRange.Remove(other.gameObject);
            if (enemiesInRange.Count == 0)
            {
                anim.SetBool("active", false);
            }
        }
    }
}
