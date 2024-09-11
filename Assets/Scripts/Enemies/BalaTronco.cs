using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaTronco : MonoBehaviour
{
    
    public float speed = 2f;

    public float timeLife = 4f;

    float damage;

    private void Start()
    {
        Destroy(gameObject, timeLife);
    }


    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    public void setDamage(float dmg)
    {
        damage = dmg;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            //collision.GetComponent<Player>().takeDamage(damage);
        }
        Destroy(gameObject);
    }
}
