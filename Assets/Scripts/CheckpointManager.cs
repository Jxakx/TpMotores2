using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Vector3 lastCheckpointPosition;

    // Al empezar, almacenamos la posici�n inicial del jugador como el primer "checkpoint".
    void Start()
    {
        lastCheckpointPosition = FindObjectOfType<Player>().transform.position;
    }

    // Actualiza la posici�n del �ltimo checkpoint alcanzado.
    public void UpdateCheckpoint(Vector3 newCheckpointPosition)
    {
        lastCheckpointPosition = newCheckpointPosition;
        Debug.Log("Checkpoint actualizado: " + lastCheckpointPosition);
    }

    // M�todo para reposicionar al jugador en el �ltimo checkpoint.
    public Vector3 GetLastCheckpointPosition()
    {
        return lastCheckpointPosition;
    }
}
