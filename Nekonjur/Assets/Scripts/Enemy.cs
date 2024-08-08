using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public UiManager uiManager;

    NavMeshAgent agent;
    public GameObject[] target;
    public Transform targetPos;
    public PlayerMovement playerMovement;
    Animator animator;

    public ParticleSystem effect;

    public enum AgentType { Wandering, Fighting };
    public AgentType agentType;

    public float detectionRange = 10f;
    public float catchRange = 2f;
    Vector3 wanderTarget;

    public float attackCooldown = 2.0f;
    private float lastAttackTime;

    public int currHealth = 0;
    public int maxHealth = 1000;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();

        currHealth = maxHealth;
    }

    #region AI Behaviors
    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Pursue() // Includes predicting where target may go
    {
        Vector3 targetDirection = target[0].transform.position - this.transform.position;
        float lookAhead = targetDirection.magnitude / (agent.speed + playerMovement.currSpeed);
        Seek(target[0].transform.position + target[0].transform.forward * lookAhead);
    }

    void Wander()
    {
        float wanderRadius = 25;
        float wanderDistance = 10;
        float wanderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                    0,
                                    Random.Range(-1.0f, 1.0f) * wanderJitter);

        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = transform.TransformPoint(targetLocal);

        Seek(targetWorld);
    }

    void Attack()
    {
        foreach (GameObject t in target)
        {
            CatBattle targetScript = t.GetComponentInChildren<CatBattle>();
            if (targetScript != null)
            {
                targetScript.GetDamage(55); // Modify as needed for different damage values
            }
            else
            {
                Debug.LogWarning("CatBattle script not found on target: " + t.name);
            }
        }
    }

    private Coroutine currentAttackRoutine = null;

    IEnumerator StartAttack(float pauseTime)
    {
        animator.SetBool("canAttack", true);
        Attack();
        yield return new WaitForSeconds(pauseTime);
        animator.SetBool("canAttack", false);
    }

    #endregion

    public void GetDamage(int damage)
    {
        currHealth -= damage;

        if (currHealth <= 0)
        {
            currHealth = 0;
            StartCoroutine(DeathTimer());
        }

        UpdateHealth();
    }

    public void UpdateHealth()
    {
        uiManager.UpdateEnemyHealth(currHealth, maxHealth);
    }

    IEnumerator DeathTimer()
    {
        UpdateHealth();
        uiManager.PrintVictory();
        animator.SetTrigger("isDead");
        yield return new WaitForSeconds(7f);
        gameObject.SetActive(false);
    }

    bool IsTargetWithinRange()
    {
        foreach (GameObject t in target)
        {
            float distanceToTarget = Vector3.Distance(this.transform.position, t.transform.position);
            if (distanceToTarget <= detectionRange)
            {
                return true; // Return true as soon as one target is within range
            }
        }
        return false;
    }

    bool IsTargetCaught()
    {
        foreach (GameObject t in target)
        {
            float distanceToTarget = Vector3.Distance(this.transform.position, t.transform.position);
            if (distanceToTarget <= catchRange)
            {
                return true; // Return true as soon as one target is caught
            }
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, catchRange);
    }

    // Update is called once per frame
    void Update()
    {
        switch(agentType)
        {
            case AgentType.Wandering:

                if (IsTargetWithinRange())
                {
                    animator.SetTrigger("isChasing");
                    Pursue();

                    if (IsTargetCaught())
                    {
                        animator.SetTrigger("playerCaught");
                        SceneManager.LoadScene("BattleGroundScene");
                    }
                }
                else
                {
                    animator.SetTrigger("isWandering");
                    Wander();
                }
                break;
            case AgentType.Fighting:

                if (IsTargetCaught() && Time.time > lastAttackTime + attackCooldown)
                {
                    if (currentAttackRoutine != null)
                        StopCoroutine(currentAttackRoutine);

                    currentAttackRoutine = StartCoroutine(StartAttack(2f));
                    lastAttackTime = Time.time;
                }
                break;
        }
       
    }
}
