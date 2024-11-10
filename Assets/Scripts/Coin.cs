using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int coinValue = 1; // Valor de cada moneda, por si deseas cambiarlo

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Aseg�rate de que el jugador tiene el tag "Player"
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.CollectCoin(); // A�ade una moneda al jugador
                Destroy(gameObject); // Destruye la moneda tras recogerla
            }
        }
    }
}
