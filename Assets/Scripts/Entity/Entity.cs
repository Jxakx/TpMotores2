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
