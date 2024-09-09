using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tronco : MonoBehaviour
{
    [SerializeField] Transform shootController;
    [SerializeField] float distance;
    [SerializeField] LayerMask player;
    [SerializeField] bool playerInRange;

    private void Update()
    {
        playerInRange = Physics2D.Raycast(shootController.position, transform.right, distance, player);

        if (playerInRange)
        {

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(shootController.position, shootController.position + transform.right * distance);
    }
}
