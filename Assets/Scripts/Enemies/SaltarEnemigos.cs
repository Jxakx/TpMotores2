using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaltarEnemigos : MonoBehaviour
{
    public int damage = 1;
    private Animator Animator;


    [SerializeField] private float cantidadPuntos;
    [SerializeField] private Puntaje puntaje;

    [SerializeField] private GameObject efecto;

    private void Start()
    {
        Animator = GetComponent<Animator>();

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            foreach (ContactPoint2D punto in other.contacts)
            {
                if(punto.normal.y <= -0.9)
                {
                    Animator.SetTrigger("Golpe");
                    other.gameObject.GetComponent<Player>().Rebound();
                }
                else if (Mathf.Abs(punto.normal.x) > 0.5f)
                {
                    other.gameObject.GetComponent<Player>().TakeDamage(1, other.GetContact(0).normal); //Da�o del knockback, cuando el player viene del costado a un enemigo, le hace da�o
                }
            }
        }
    }

    public void Golpe()
    {
        Instantiate(efecto, transform.position, transform.rotation);
        Destroy(gameObject);
        puntaje.SumarPuntos(cantidadPuntos);

    }
}
