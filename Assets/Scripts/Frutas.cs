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
            // Obtener referencia al script del jugador
            Player player = other.GetComponent<Player>();

            // Sumar puntos al puntaje
            puntaje.SumarPuntos(cantidadPuntos);

            // Curar al jugador
            if (player != null)
            {
                player.Curar(curacion);
            }

            // Instanciar efecto visual y destruir la fruta
            Instantiate(efecto, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
