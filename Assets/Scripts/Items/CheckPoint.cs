using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Animator animatorCheckpoint;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CheckpointManager checkpointManager = FindObjectOfType<CheckpointManager>();
            if (checkpointManager != null)
            {
                checkpointManager.UpdateCheckpointPosition(transform.position);
                Debug.Log("Jugador ha activado el checkpoint en: " + transform.position);

            }

            animatorCheckpoint.SetBool("Checking", true);

            //Destroy(gameObject); // Destruye el checkpoint después de activarlo
        }
    }
}
