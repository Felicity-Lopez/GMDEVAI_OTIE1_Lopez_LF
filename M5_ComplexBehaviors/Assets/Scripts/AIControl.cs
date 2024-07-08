using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;

    public WASDMovement playerMovement;

    public enum AgentType { Agent1, Agent2, Agent3 };
    public AgentType agentType;

    public float range = 10f;
    Vector3 wanderTarget;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        playerMovement = target.GetComponent<WASDMovement>();
    }

    #region AI Behaviors
    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeDirection = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeDirection);
    }

    void Pursue() // Includes predicting where target may go
    {
        Vector3 targetDirection = target.transform.position - this.transform.position;
        float lookAhead = targetDirection.magnitude/(agent.speed + playerMovement.currentSpeed);
        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    void Evade() // Includes predicting where target may go
    {
        Vector3 targetDirection = target.transform.position - this.transform.position;
        float lookAhead = targetDirection.magnitude / (agent.speed + playerMovement.currentSpeed);
        Flee(target.transform.position + target.transform.forward * lookAhead);
    }

    void Wander()
    {
        float wanderRadius = 20;
        float wanderDistance = 10;
        float wanderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                    0,
                                    Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    void Hide()
    {
        // Find nearest hiding spot
        float distance = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        int hidingSpotsCount = World.Instance.GetHidingSpots().Length;

        for (int i = 0; i < hidingSpotsCount; i++)
        {
            Vector3 hideDirection = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
            Vector3 hidePosition = World.Instance.GetHidingSpots()[i].transform.position + hideDirection.normalized * 5; // Distance offset

            float spotDistance = Vector3.Distance(this.transform.position, hidePosition);
            if (spotDistance < distance)
            {
                chosenSpot = hidePosition;
                distance = spotDistance;
            }
        }

        Seek(chosenSpot);
    }

    void CleverHide()
    {
        // Find nearest hiding spot
        float distance = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDirection = Vector3.zero;
        GameObject chosenGameObject = World.Instance.GetHidingSpots()[0];

        int hidingSpotsCount = World.Instance.GetHidingSpots().Length;

        for (int i = 0; i < hidingSpotsCount; i++)
        {
            Vector3 hideDirection = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
            Vector3 hidePosition = World.Instance.GetHidingSpots()[i].transform.position + hideDirection.normalized * 5; // Distance offset

            float spotDistance = Vector3.Distance(this.transform.position, hidePosition);
            if (spotDistance < distance)
            {
                chosenSpot = hidePosition;
                chosenDirection = hideDirection;
                chosenGameObject = World.Instance.GetHidingSpots()[i];
                distance = spotDistance;
            }
        }

        Collider hideCol = chosenGameObject.GetComponent<Collider>();
        Ray back = new Ray(chosenSpot.normalized, -chosenDirection.normalized);
        RaycastHit info;
        float rayDistance = 100.0f;
        hideCol.Raycast(back, out info, rayDistance);

        Seek(info.point + chosenDirection.normalized * 5);
    }

    #endregion
    
    bool CanSeeTarget()
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = target.transform.position - this.transform.position;

        if (Physics.Raycast(this.transform.position, rayToTarget, out raycastInfo))
        {

            return raycastInfo.transform.gameObject.tag == "Player";
        }
        return false;
    }

    bool IsTargetWithinRange()
    {
        float distanceToTarget = Vector3.Distance(this.transform.position, target.transform.position);
        return distanceToTarget <= range;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, range);
    }

    // Update is called once per frame
    void Update()
    {
        switch (agentType)
        {
            case AgentType.Agent1:
                if (IsTargetWithinRange())
                {
                    Pursue();
                }
                else
                {
                    Wander();
                }
                break;
            case AgentType.Agent2:
                if (IsTargetWithinRange() && CanSeeTarget())
                {
                    Hide();
                }
                else
                {
                    Wander();
                }
                break;
            case AgentType.Agent3:
                if (IsTargetWithinRange())
                {
                    Evade();
                }
                else
                {
                    Wander();
                }
                break;
        }
    }
}
