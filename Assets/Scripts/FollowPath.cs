using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public int damage = 1;

    Transform[] path;
    GameObject objective;
    float speed;

    int currentObjectiveIdx = -1;
    Vector3 currentObjective;

    void Start()
    {

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

    public void SetSpeed(float speed) {
        this.speed = speed;
    }

    public void SetPath(Transform[] path, GameObject objective, float speed) {
        this.path = path;
        this.objective = objective;
        this.speed = speed;

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
