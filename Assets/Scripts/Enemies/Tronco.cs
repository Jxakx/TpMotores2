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

    private void Update()
    {
        
        playerInRange = Physics2D.Raycast(shootController.position, -transform.right, distance, player);

        if (playerInRange)
        {
            if (Time.time > lastShoot + waitShootTime)
            {
                lastShoot = Time.time;
                Shoot();
            }
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
