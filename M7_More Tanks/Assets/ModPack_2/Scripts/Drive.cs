using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Drive : MonoBehaviour {

    public GameObject bullet;
    public GameObject turret;

 	public float speed = 10.0F;
    public float rotationSpeed = 100.0F;

    public TextMeshProUGUI HP;
    public int currHealth = 0;
    public int maxHealth = 100;

    private void Awake()
    {
        currHealth = maxHealth;
    }

    void Update() {

        if (currHealth <= 0) 
        {
            Destroy(this.gameObject);
        }

        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    void Fire()
    {
        GameObject b = Instantiate(bullet, turret.transform.position, turret.transform.rotation);
        b.GetComponent<Rigidbody>().AddForce(turret.transform.forward * 500);
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
        HP.SetText("Player: \n" + currHealth + " / " + maxHealth);
    }
}
