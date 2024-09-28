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

    private int shootMode = 0; // 0 para disparo normal, 1 para disparo triple
    private int tripleShotCount = 0; // Contador para el disparo triple
    private float tripleShotDelay = 0.2f; // Intervalo entre balas en disparo triple
    private float lastTripleShotTime;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Comprobar si el jugador está en rango, ya sea delante o detrás
        playerInRange = Physics2D.Raycast(shootController.position, -transform.right, distance, player) ||
                        Physics2D.Raycast(shootController.position, transform.right, distance, player);

        // Siempre rotar hacia el jugador si está detrás o enfrente
        RotateTowardsPlayer();

        if (playerInRange)
        {
            // Solo dispara si el jugador está en rango
            if (Time.time > lastShoot + waitShootTime)
            {
                lastShoot = Time.time;

                if (shootMode == 0)
                {
                    Shoot();
                    shootMode = 1; // Cambiar al modo de disparo triple en el siguiente ciclo
                }
                else if (shootMode == 1 && tripleShotCount == 0)
                {
                    // Comenzar disparo triple
                    tripleShotCount = 3; // Disparar 3 balas
                    lastTripleShotTime = Time.time;
                    Shoot(); // Primer disparo
                }
            }

            // Si está en modo de disparo triple, manejar los disparos consecutivos
            if (shootMode == 1 && tripleShotCount > 0)
            {
                if (Time.time > lastTripleShotTime + tripleShotDelay)
                {
                    lastTripleShotTime = Time.time;
                    Shoot();
                    tripleShotCount--;

                    // Si ya disparó las 3 balas, volver al disparo normal
                    if (tripleShotCount == 0)
                    {
                        shootMode = 0;
                    }
                }
            }
        }
    }

    private void RotateTowardsPlayer()
    {
        // Calcular la dirección hacia el jugador
        Vector2 directionToPlayer = playerTransform.position - transform.position;

        // Si el jugador está en el lado opuesto, rotar hacia él
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
        float bulletRotation = transform.localScale.x > 0 ? 0f : 180f;
        Instantiate(bulletEnemy, shootController.position, Quaternion.Euler(0f, bulletRotation, 0f));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(shootController.position, shootController.position + -transform.right * distance);
        Gizmos.DrawLine(shootController.position, shootController.position + transform.right * distance);
    }
}
