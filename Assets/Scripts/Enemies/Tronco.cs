using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tronco : Entity
{
    [SerializeField] Transform shootController;
    [SerializeField] float distance;
    [SerializeField] LayerMask player;
    [SerializeField] bool playerInRange;

    public float timer;
    public float lastShoot;
    public float waitShootTime;
    public GameObject bulletEnemy;

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Comprobar si el jugador est� en rango, ya sea delante o detr�s
        playerInRange = Physics2D.Raycast(shootController.position, -transform.right, distance, player) ||
                        Physics2D.Raycast(shootController.position, transform.right, distance, player);

        // Siempre rotar hacia el jugador si est� detr�s o enfrente
        RotateTowardsPlayer();

        if (playerInRange)
        {
            // Solo dispara si el jugador est� en rango
            if (Time.time > lastShoot + waitShootTime)
            {
                lastShoot = Time.time;
                Shoot();
            }
        }
    }

    private void RotateTowardsPlayer()
    {
        // Calcular la direcci�n hacia el jugador en 2D
        Vector2 directionToPlayer = playerTransform.position - transform.position;

        // Si el jugador est� en el lado opuesto, rotar hacia �l
        if ((directionToPlayer.x > 0 && transform.localScale.x < 0) || (directionToPlayer.x < 0 && transform.localScale.x > 0))
        {
            // Invertir la escala en el eje X para voltear al enemigo
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }

    private void Shoot()
    {
        // Asegurarse de que la bala se instancie en la direcci�n correcta seg�n la rotaci�n del enemigo
        float bulletRotation = transform.localScale.x > 0 ? 0f : 180f;
        Instantiate(bulletEnemy, shootController.position, Quaternion.Euler(0f, bulletRotation, 0f));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Dibujar el raycast hacia ambos lados del enemigo
        Gizmos.DrawLine(shootController.position, shootController.position + -transform.right * distance);
        Gizmos.DrawLine(shootController.position, shootController.position + transform.right * distance);
    }
}
