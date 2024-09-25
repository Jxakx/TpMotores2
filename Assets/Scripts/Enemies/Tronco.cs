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
        
        playerInRange = Physics2D.Raycast(shootController.position, -transform.right, distance, player);

        if (playerInRange)
        {
            RotateTowardsPlayer();
            if (Time.time > lastShoot + waitShootTime)
            {
                lastShoot = Time.time;
                Shoot();
            }
        }
    }

    private void RotateTowardsPlayer()
    {
        // Calcular la dirección hacia el jugador en 2D
        Vector2 directionToPlayer = playerTransform.position - transform.position;

        // Si el jugador está detrás (en el lado opuesto) del enemigo, rotar hacia él
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
        Instantiate(bulletEnemy, shootController.position, Quaternion.Euler(0f, 180f, 0f));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawLine(shootController.position, shootController.position + -transform.right * distance);
    }
}
