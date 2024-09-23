using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltarEnemigos : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach(ContactPoint2D punto in other.contacts)
            {
                if(punto.normal.y <= -0.9)
                {
                    other.gameObject.GetComponent<Player>().Rebound();
                }
                else if (Mathf.Abs(punto.normal.x) > 0.5f)
                {
                    other.gameObject.GetComponent<Player>().TakeDamage(damage);
                }
            }
        }
    }
}
