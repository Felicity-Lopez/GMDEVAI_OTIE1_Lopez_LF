using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public GameObject[] goalLocations;
    NavMeshAgent agent;
    Animator animator;
    float speedMultiplier;

    float detectionRadius = 20;
    float moveRadius = 10;

    void ResetAgent()
    {
        speedMultiplier = Random.Range(0.1f, 1.5f);
        agent.speed = 2 * speedMultiplier;
        agent.angularSpeed = 120;
        animator.SetFloat("speedMultiplier", speedMultiplier);
        animator.SetTrigger("isWalking");
        agent.ResetPath();
    }

    void Start()
    {
        goalLocations = GameObject.FindGameObjectsWithTag("goal");
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();

        agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        animator.SetFloat("wOffset", Random.Range(0.1f, 1f));
        ResetAgent();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < 1)
        {
            ResetAgent();
            agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        }
    }

    void CreatePath(Vector3 newGoal)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(newGoal, path);

        if (path.status != NavMeshPathStatus.PathInvalid)
        {
            agent.SetDestination(path.corners[path.corners.Length - 1]);
            animator.SetTrigger("isRunning");
            agent.speed = 10;
            agent.angularSpeed = 500;
        }
    }

    public void DetectNewObstacle(Vector3 location, bool isLeftClick)
    {   
        Vector3 direction = Vector3.zero;

        if (Vector3.Distance(location, this.transform.position) < detectionRadius)
        {
            if (isLeftClick) 
            { 
                direction = (this.transform.position - location).normalized;
            } 
            
            if (!isLeftClick) 
            {
                direction = (location - this.transform.position).normalized;
            }
                
            Vector3 newGoal = this.transform.position + direction * moveRadius;
            CreatePath(newGoal);
        }
    }
}
