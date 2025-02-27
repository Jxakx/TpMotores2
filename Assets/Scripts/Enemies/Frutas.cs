using System.Collections;
using UnityEngine;

public class Frutas : MonoBehaviour
{
    [SerializeField] private GameObject efecto;
    [SerializeField] private float cantidadPuntos;
    [SerializeField] private Puntaje puntaje;
    [SerializeField] private AudioSource eatSound;
    [SerializeField] private int curacion;
    private bool hasBeenCollected = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasBeenCollected)
        {
            if (eatSound != null)
            {
                eatSound.Play();
            }

            puntaje.SumarPuntos(cantidadPuntos);

            hasBeenCollected = true;

            // Curar al Player
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Curar(curacion);
            }

            Instantiate(efecto, transform.position, transform.rotation);
        }

        // Desactivar el renderer y el collider de la moneda para que no se pueda recolectar nuevamente

        if (other.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
        
    }
}