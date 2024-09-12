using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] int life;
    [SerializeField] Controller controller;
    [SerializeField] float speed = 5;
    [SerializeField] float smoothedMove;
    private bool lookRight = true;

    [Header("Salto")]

    [SerializeField] LayerMask floor;
    [SerializeField] Transform floorController;
    [SerializeField] Vector3 boxDimensions;
    [SerializeField] float jumpForce = 5f;
    private bool isGrounded = false;

    [Header("Dash")]

    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    private float starterGravity;
    private bool canDash = true;
    private bool canMove = true;

    [Header("DoubleJump")]

    [SerializeField] private int saltosExtraRestantes;
    [SerializeField] private int saltosExtra;

    [Header("Dash")]

    [SerializeField] float speedRebound;

    [Header("Animaciones")]

    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        starterGravity = rb.gravityScale;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapBox(floorController.position, boxDimensions, 0f, floor);

        if(isGrounded)
        {
            // Reseteamos el parámetro de doble salto al estar en el suelo
            saltosExtraRestantes = saltosExtra;

            animator.SetBool("isDoubleJumping", false);

        }

        // Actualizamos el parámetro "enSuelo" en el Animator
        animator.SetBool("enSuelo", isGrounded);

    }

    private void FixedUpdate()
    {
        // Movemos al jugador en función de la entrada del controlador
        

        // Actualizar el parámetro "Horizontal" del Animator según la velocidad
        animator.SetFloat("Horizontal", Mathf.Abs(rb.velocity.x));

        animator.SetFloat("VelocidadY", rb.velocity.y);

        animator.SetBool("isDoubleJumping", false);

        // Si se presiona el botón de salto y el jugador está en el suelo, saltamos
        if (controller.IsJumping())
        {
            if (isGrounded)
            {
                Jump();
            }
            else
            {
                if(saltosExtraRestantes > 0)
                {
                    Jump();
                    saltosExtraRestantes -= 1; // Restar un salto extra

                    //Activamos el parámetro para la animación de doble salto
                    animator.SetBool("isDoubleJumping", true);

                }              
            }
        }
       

        if (canMove)
        {
            rb.velocity = new Vector2(controller.GetMoveDir().x * speed, rb.velocity.y);
            Move();
        }

        if(controller.IsDashing() && canDash)
        {
            StartCoroutine(Dash());
        }

        // Gestionamos la dirección en la que mira el personaje
        
    }
    private void Move()
    {
        float move = rb.velocity.x;

        if (move > 0 && !lookRight)
        {
            Turn();
        }else if(move < 0 && lookRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        lookRight = !lookRight;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private IEnumerator Dash()
    {
        canMove = false;
        canDash = false;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(dashSpeed * transform.localScale.x, 0);

        yield return new WaitForSeconds(dashTime);

        canMove = true;
        canDash = true;
        rb.gravityScale = starterGravity;
    }

    public void Rebound()
    {
        rb.velocity = new Vector2(rb.velocity.x, speedRebound);
    }
    public void TakeDamage(int value)
    {
        life -= value;
        //_gameManager.LoseHP(value);

        if (life <= 0)
        {
            life = 0;
            Dead();
        }
    }

    private void Dead()
    {
        Time.timeScale = 0;
        //gamePlayCanvas.onLose();

        Destroy(GetComponent<Player>(), 1);
    }

    // Opcional: Para visualizar el OverlapBox en el editor y depurar problemas de detección
    private void OnDrawGizmos()
    {
        if (floorController != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(floorController.position, boxDimensions);
        }
    }
}
