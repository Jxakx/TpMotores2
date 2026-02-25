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

    private int shootMode = 0;
    private int tripleShotCount = 0;
    private float tripleShotDelay = 0.2f;
    private float lastTripleShotTime;
    public AudioSource shootSound;

    public Animator animator;
    public float tiempoEsperaDisparo;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        playerInRange = Physics2D.Raycast(shootController.position, -transform.right, distance, player) ||
                        Physics2D.Raycast(shootController.position, transform.right, distance, player);

        RotateTowardsPlayer();

        if (playerInRange)
        {
            if (Time.time > lastShoot + waitShootTime)
            {
                lastShoot = Time.time;
                animator.SetTrigger("Disparar");
                Invoke(nameof(ShootLogic), tiempoEsperaDisparo);
            }
        }

        if (shootMode == 1 && tripleShotCount > 0)
        {
            if (Time.time > lastTripleShotTime + tripleShotDelay)
            {
                lastTripleShotTime = Time.time;
                Shoot();
                tripleShotCount--;

                if (tripleShotCount == 0)
                {
                    shootMode = 0;
                }
            }
        }
    }

    private void ShootLogic()
    {
        if (shootMode == 0)
        {
            Shoot();
            shootMode = 1;
            print("Disparo 1");
        }
        else if (shootMode == 1 && tripleShotCount == 0)
        {
            tripleShotCount = 3;
            lastTripleShotTime = Time.time;
            print("Disparo triple iniciado");
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector2 directionToPlayer = playerTransform.position - transform.position;

        if ((directionToPlayer.x > 0 && transform.localScale.x < 0) || (directionToPlayer.x < 0 && transform.localScale.x > 0))
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }

    private void Shoot()
    {
        float bulletRotation = transform.localScale.x > 0 ? 0f : 180f;
        Instantiate(bulletEnemy, shootController.position, Quaternion.Euler(0f, bulletRotation, 0f));
        shootSound.Play();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(shootController.position, shootController.position + -transform.right * distance);
        Gizmos.DrawLine(shootController.position, shootController.position + transform.right * distance);
    }
}