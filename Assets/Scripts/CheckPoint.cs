using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Checkpoint alcanzado");
            FindObjectOfType<CheckpointManager>().UpdateCheckpoint(transform.position);
            Destroy(gameObject); // Eliminar el checkpoint una vez alcanzado
        }
    }
}
