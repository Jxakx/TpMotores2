using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Vector3 lastCheckpointPosition;

    // Al empezar, almacenamos la posición inicial del jugador como el primer "checkpoint".
    void Start()
    {
        lastCheckpointPosition = FindObjectOfType<Player>().transform.position;
    }

    // Actualiza la posición del último checkpoint alcanzado.
    public void UpdateCheckpoint(Vector3 newCheckpointPosition)
    {
        lastCheckpointPosition = newCheckpointPosition;
        Debug.Log("Checkpoint actualizado: " + lastCheckpointPosition);
    }

    // Método para reposicionar al jugador en el último checkpoint.
    public Vector3 GetLastCheckpointPosition()
    {
        return lastCheckpointPosition;
    }
}
