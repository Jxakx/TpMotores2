using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frutas : MonoBehaviour
{
    [SerializeField] private GameObject efecto;
    [SerializeField] private float cantidadPuntos;
    [SerializeField] private Puntaje puntaje;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            puntaje.SumarPuntos(cantidadPuntos);
            Instantiate(efecto, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
