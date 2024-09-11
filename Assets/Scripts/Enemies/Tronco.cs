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
        playerInRange = Physics2D.Raycast(shootController.position, transform.right, distance, player);

        if (playerInRange)
        {
            if(Time.time > timer + lastShoot)
            {
                lastShoot = Time.time;
                Invoke(nameof(Shoot), waitShootTime);
            }
        }
    }

    private void Shoot()
    {
        Instantiate(bulletEnemy, shootController.position, shootController.rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(shootController.position, shootController.position + transform.right * distance);
    }
}
