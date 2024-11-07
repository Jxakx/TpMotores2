using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int life;
    public int damageAttack;


    void Start()
    {
        
    }

 
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) //Para el Knowback
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().TakeDamage(1, other.GetContact(0).normal);
        }
    }

    public void TakeDamage(int damage)
    {

        life -= damage;

        if (life <= 1.5f)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }

   

}
