using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class KingAI : MonoBehaviour
{
    public enum FSMStates
    {
        Wander,
        Chase,
        Dialogue
    }

    public FSMStates currentState;

    GameObject[] wanderPoints;
    Vector3 nextDestination;
    float distanceToPlayer;

    public float chaseDistance = 10f;
    public float dialogueRange = 5f;

    public Transform npcEyes;
    public float fov = 200f;

    int currentDestinationIndex = 0;

    public GameObject player; 
    private NavMeshAgent agent;
    private Animator anim;

    private NPCText text;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        anim = GetComponent<Animator>();
        anim.SetInteger("WeaponType_int", 0);

        currentState = FSMStates.Wander;

        player = GameObject.FindGameObjectWithTag("Player");
        wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");

        agent.SetDestination(player.transform.position);
        agent.stoppingDistance = 0f;

        text = GetComponentInChildren<NPCText>();
    }

    void Update()
    {

        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position); ;

        switch (currentState)
        {
            case FSMStates.Wander:
                UpdateWanderState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Dialogue:
                UpdateDialogueState();
                break;
        }
    }

    void UpdateWanderState()
    {
        text.ShowText(false);
        agent.speed = 3f;
        anim.SetFloat("Speed_f", 0.4f);
        anim.SetInteger("Animation_int", 0);
        agent.stoppingDistance = 1f;

        if (Vector3.Distance(transform.position, nextDestination) < 2)
        {
            FindNextPoint();
        }
        else if (IsPlayerInClearFOV())
        {
            if (distanceToPlayer <= dialogueRange)
            {
                currentState = FSMStates.Dialogue;
            } else
            {
                currentState = FSMStates.Chase;
            }
        }

        FaceTarget(nextDestination);

        agent.SetDestination(nextDestination);

    }

    void UpdateChaseState()
    {
        text.ShowText(false);
        agent.speed = 4f;
        agent.SetDestination(player.transform.position);
        anim.SetFloat("Speed_f", 0.7f);
        anim.SetInteger("Animation_int", 0);
        agent.SetDestination(nextDestination);
        agent.stoppingDistance = dialogueRange;

        //anim.SetInteger("animState", 2);
        nextDestination = player.transform.position;
        agent.stoppingDistance = dialogueRange;

        if (distanceToPlayer <= dialogueRange && IsPlayerInClearFOV())
        {
            currentState = FSMStates.Dialogue;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            FindNextPoint();
            currentState = FSMStates.Wander;
        }

        FaceTarget(nextDestination);
    }

    void UpdateDialogueState()
    {
        //print("attack");

        text.ShowText(true);
        nextDestination = player.transform.position;
        agent.stoppingDistance = dialogueRange;

        anim.SetFloat("Speed_f", 0f);
        anim.SetInteger("Animation_int", 1);

        if (distanceToPlayer <= dialogueRange)
        {
            currentState = FSMStates.Dialogue;
        }
        else if (distanceToPlayer > dialogueRange && distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Wander;
        }

        FaceTarget(nextDestination);

    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    void FindNextPoint()
    {
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;

        currentDestinationIndex =
            (currentDestinationIndex + 1)
            % wanderPoints.Length;

        agent.SetDestination(nextDestination);
    }

    bool IsPlayerInClearFOV()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = player.transform.position - npcEyes.position;

        if (Vector3.Angle(directionToPlayer, npcEyes.forward) <= fov)
        {
            if (Physics.Raycast(npcEyes.position, directionToPlayer, out hit, chaseDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    //print("player seen");
                    return true;
                }
                return false;
            }
        }
        return false;
    }
}
