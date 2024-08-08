using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class CatBattle : MonoBehaviour
{
    public UiManager uiManager;
    public Animator animator;

    public enum Class
    {
        Tank,
        DPS,
        Support
    }

    public Class role;

    public GameObject target;
    public Transform targetPos;
    public Enemy targetScript;

    public int currHealth = 0;
    public int maxHealth = 0;
    public int damage = 0;

    public float detectionRange = 2f;

    public float skillCooldown = 5f;
    public int mana = 10;
    public bool isAttacking = false;
    public Slider manaSlider;

    // Start is called before the first frame update
    void Start()
    {
        targetScript = target.GetComponent<Enemy>();

        switch (role)
        {
            case Class.Tank:
                maxHealth = 600;
                damage = 30;
                manaSlider = null;
                break; 

            case Class.DPS:
                maxHealth = 350;
                damage = 50;
                manaSlider = null;
                break;

            case Class.Support:
                maxHealth = 200;
                damage = 5;
                manaSlider.value = 10;
                break;
        }

        currHealth = maxHealth;
    }

    public void GetDamage(int damage)
    {
        currHealth -= damage;

        if (currHealth <= 0)
        {
            switch (role)
            {
                case Class.Tank:
                    uiManager.tankEliminated.SetActive(true);
                    break;

                case Class.DPS:
                    uiManager.dpsEliminated.SetActive(true);
                    break;

                case Class.Support:
                    uiManager.supportEliminated.SetActive(true);
                    break;
            }

            gameObject.SetActive(false);
        }

        UpdateHealth();
    }

    public void GetHeal(int heal)
    {
        currHealth += heal;

        if (currHealth > maxHealth)
        {
            currHealth = maxHealth;
        }

        UpdateHealth();
    }

    public void UpdateHealth()
    {
        switch (role)
        {
            case Class.Tank:
                uiManager.UpdateTankHealth(currHealth, maxHealth);
                break;

            case Class.DPS:
                uiManager.UpdateDpsHealth(currHealth, maxHealth);
                break;

            case Class.Support:
                uiManager.UpdateSupportHealth(currHealth, maxHealth);
                break;
        }
    }

    bool IsTargetWithinRange()
    {
        float distanceToTarget = Vector3.Distance(this.transform.position, target.transform.position);
        return distanceToTarget <= detectionRange;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, detectionRange);
    }

    IEnumerator Attack()
    {
        animator.SetBool("canAttack", true);
        targetScript.GetDamage(damage);
        yield return new WaitForSeconds(skillCooldown);
        animator.SetBool("canAttack", false);
        isAttacking = false;
    }

    public IEnumerator Heal()
    {
        mana--;
        manaSlider.value = mana;
        animator.SetBool("canHeal", true);
        yield return new WaitForSeconds(skillCooldown);
        animator.SetBool("canHeal", false);
    }

    public void PlayHealAnim()
    {
        if (mana > 0)
        {
            StartCoroutine(Heal());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && !isAttacking && IsTargetWithinRange()) // Attack
        {
            isAttacking = true;
            StartCoroutine(Attack());
        }
    }
}
