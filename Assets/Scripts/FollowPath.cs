using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public float speed = 1;
    public int damage = 1;

    Animator animator;

    Transform[] path;
    GameObject objective;

    int currentObjectiveIdx = -1;
    Vector3 currentObjective;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("WeaponType_int", 0);
        animator.SetFloat("Speed_f", 0.3f);
    }

    void Update()
    {
        if (currentObjectiveIdx >= 0) {
            if (Vector3.Distance(transform.position, currentObjective) == 0) {
                NextObjective();
            }

            transform.position = Vector3.MoveTowards(transform.position, currentObjective, Time.deltaTime * speed);
        }
    }

    public void SetPath(Transform[] path, GameObject objective) {
        this.path = path;
        this.objective = objective;

        currentObjectiveIdx = -1;

        NextObjective();
    }

    void NextObjective() {
        currentObjectiveIdx++;
        
        if (currentObjectiveIdx < path.Length) {
            currentObjective = path[currentObjectiveIdx].position;
        } else if (currentObjectiveIdx == path.Length) {
            currentObjective = objective.transform.position;
        } else {
            objective.GetComponent<ObjectiveHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }

        transform.LookAt(currentObjective);
    }
}
