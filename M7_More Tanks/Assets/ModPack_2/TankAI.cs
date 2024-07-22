using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TankAI : MonoBehaviour
{
    Animator anim;
    public GameObject player;
    public GameObject bullet;
    public GameObject turret;

    public TextMeshProUGUI HP;

    public int currHealth = 0;
    public int maxHealth = 100;

    public GameObject GetPlayer()
    { return player; }

    private void Awake()
    {
        currHealth = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currHealth <= 0)
        {
            Destroy(this.gameObject);
        }

        anim.SetFloat("distance", Vector3.Distance(transform.position, player.transform.position));
    }

    void Fire()
    {
        GameObject b = Instantiate(bullet, turret.transform.position, turret.transform.rotation);
        b.GetComponent<Rigidbody>().AddForce(turret.transform.forward * 500);
    }

    public void StopFiring()
    {
        CancelInvoke("Fire");
    }

    public void StartFiring()
    {
        InvokeRepeating("Fire", 0.5f, 0.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currHealth > 0)
        {
            SetHealth(5);
        }
    }

    private void SetHealth(int damage)
    {
        currHealth -= damage;
        anim.SetInteger("HP", currHealth);
        HP.SetText("Enemy: \n" + currHealth + " / " + maxHealth);
    }
}
