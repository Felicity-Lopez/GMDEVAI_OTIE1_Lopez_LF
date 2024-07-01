using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;

    public Transform player;
    public float stoppingDistance = 2f;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
    }

    void Update()
    {
        Vector3 lookAtPlayer = new Vector3(player.position.x,
                                         this.transform.position.y,
                                         player.position.z);

        float distanceToGoal = Vector3.Distance(lookAtPlayer, transform.position);

        if (player != null && distanceToGoal > agent.stoppingDistance)
        {
            agent.SetDestination(player.position);
        }
    }
}
