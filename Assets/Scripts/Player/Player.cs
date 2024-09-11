using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] Controller controller;
    [SerializeField] float speed = 5;
    [SerializeField] float smoothedMove;
    private bool lookRight = true;

    
    [SerializeField] LayerMask floor;
    [SerializeField] Transform floorController;
    [SerializeField] Vector3 boxDimensions;
    [SerializeField] float jumpForce = 5f;
    private bool isGrounded = false;

    [Header("Animaciones")]

    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapBox(floorController.position, boxDimensions, 0f, floor);

        // Actualizamos el parámetro "enSuelo" en el Animator
        animator.SetBool("enSuelo", isGrounded);
    }

    private void FixedUpdate()
    {
        // Movemos al jugador en función de la entrada del controlador
        rb.velocity = new Vector2(controller.GetMoveDir().x * speed, rb.velocity.y);

        // Actualizar el parámetro "Horizontal" del Animator según la velocidad
        animator.SetFloat("Horizontal", Mathf.Abs(rb.velocity.x));

        animator.SetFloat("VelocidadY", rb.velocity.y);

        // Si se presiona el botón de salto y el jugador está en el suelo, saltamos
        if (controller.IsJumping() && isGrounded)
        {
            Jump();
        }

        // Gestionamos la dirección en la que mira el personaje
        Move();
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
