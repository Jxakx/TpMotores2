using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioSource coinAudioSource; // AudioSource para el sonido de la moneda
    [SerializeField] private ParticleSystem particulasDestello; // Sistema de partículas para el destello
    private bool hasBeenCollected = false; // Controla si la moneda ya fue recolectada

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el jugador ha entrado en la moneda y si no ha sido recolectada antes
        if (other.CompareTag("Player") && !hasBeenCollected)
        {
            // Reproducir el sonido y las partículas solo una vez
            if (coinAudioSource != null)
            {
                coinAudioSource.Play();
            }

            if (particulasDestello != null)
            {
                particulasDestello.Play();
            }

            // Marcar que la moneda ya fue recolectada
            hasBeenCollected = true;

            // Añadir una moneda al jugador
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.CollectCoin(); // Añade una moneda al jugador
            }

            // Desactivar el renderer y el collider de la moneda para que no se pueda recolectar nuevamente
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            // Desactivar la moneda después de un breve retraso (opcional)
            StartCoroutine(DeactivateAfterSound());
        }
    }

    private IEnumerator DeactivateAfterSound()
    {
        // Esperar a que termine el sonido (si hay un AudioSource)
        if (coinAudioSource != null)
        {
            yield return new WaitForSeconds(coinAudioSource.clip.length);
        }
        else
        {
            yield return null; // Si no hay sonido, desactivar inmediatamente
        }

        // Desactivar la moneda (pero no destruirla)
        gameObject.SetActive(false);
    }
}