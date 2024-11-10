using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frutas : MonoBehaviour
{
    [SerializeField] private GameObject efecto;
    [SerializeField] private float cantidadPuntos;
    [SerializeField] private Puntaje puntaje;
        
    [SerializeField] private int curacion;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            puntaje.SumarPuntos(cantidadPuntos);

            // Curar al Player
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Curar(curacion);
            }

            Instantiate(efecto, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
