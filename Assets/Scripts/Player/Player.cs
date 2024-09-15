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

    [Header("SaltoPared")]
    [SerializeField] private Transform controladorPared;
    [SerializeField] private Vector3 dimensionCajaPared;
    private bool enPared;
    private bool deslizando;
    [SerializeField] private float velocidadDeslizar;
    [SerializeField] private float fuerzaSaltoParedX;
    [SerializeField] private float fuerzaSaltoParedY;
    [SerializeField] private float tiempoSaltoPared;
    private bool saltandoDePared;

    [Header("Animaciones")]

    private Animator animator;

    //Para las partículas cuando corre/camina/dash
    [SerializeField] private ParticleSystem particulasDash;
    [SerializeField] private ParticleSystem particulasCorrer;
    [SerializeField] private ParticleSystem particulasAterrizaje;
    [SerializeField] private ParticleSystem particulasDj;

    private bool wasGrounded = true; //Para que cuando toque el suelo, aparezcan las particulas de aterrizaje

    [SerializeField] private AudioClip jumpSound; 
    [SerializeField] private AudioClip doubleJumpSound;
    [SerializeField] private AudioClip dashSound;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        starterGravity = rb.gravityScale;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapBox(floorController.position, boxDimensions, 0f, floor);
        enPared = Physics2D.OverlapBox(controladorPared.position, dimensionCajaPared, 0f, floor);

        if (!wasGrounded && isGrounded)
        {
            saltosExtraRestantes = saltosExtra;
            animator.SetBool("isDoubleJumping", false);
            particulasAterrizaje.Play();
        }

        wasGrounded = isGrounded;

        animator.SetBool("enSuelo", isGrounded);

        // Determina si se debe deslizar en la pared
        if (enPared && !isGrounded)
        {
            if (controller.GetMoveDir().x != 0) // Solo deslizar si se está moviendo
            {
                deslizando = true;
                animator.SetBool("Deslizando", deslizando);    
            }
            else
            {
                deslizando = false;
                animator.SetBool("Deslizando", false);
            }
        }
        else
        {
            deslizando = false;
            animator.SetBool("Deslizando", false);

        }

        if (deslizando)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -velocidadDeslizar, float.MaxValue));
        }
    }

    private void FixedUpdate()
    {
        if (!saltandoDePared)
        {
            if (canMove)
            {
                rb.velocity = new Vector2(controller.GetMoveDir().x * speed, rb.velocity.y);
                Move();
            }
        }

        if (controller.GetMoveDir().x != 0 && isGrounded)
        {
            if (!particulasCorrer.isPlaying)
            {
                particulasCorrer.Play();  // Iniciar partículas cuando corra
            }
        }
        else
        {
            if (particulasCorrer.isPlaying)
            {
                particulasCorrer.Stop();  // Detener partículas cuando no corra
            }
        }

        if (controller.IsDashing() && canDash)
        {
            StartCoroutine(Dash());
            particulasDash.Play();
            ControllerSFX.instance.executeSound(dashSound);
        }

        // Actualizar parámetros de animación
        animator.SetFloat("Horizontal", Mathf.Abs(rb.velocity.x));
        animator.SetBool("isDoubleJumping", false);
        animator.SetFloat("VelocidadY", rb.velocity.y);



        if (controller.IsJumping())
        {

            if (isGrounded && !deslizando)
            {
                Jump();
                ControllerSFX.instance.executeSound(jumpSound);
            }
            else if(enPared && deslizando)
            {
                SaltoPared();                
            }
            else
            {
                if (saltosExtraRestantes > 0)
                {
                    Jump();
                    saltosExtraRestantes -= 1;
                    animator.SetBool("isDoubleJumping", true);
                    particulasDj.Play();
                    ControllerSFX.instance.executeSound(doubleJumpSound);
                }
            }
        }
    }

    private void Move()
    {
        float move = rb.velocity.x;

        if (move > 0 && !lookRight)
        {
            Turn();
        }
        else if (move < 0 && lookRight)
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

    private void SaltoPared()
    {
        enPared = false;
        rb.velocity = new Vector2(fuerzaSaltoParedX * -controller.GetMoveDir().x, fuerzaSaltoParedY);
        StartCoroutine(CambioSaltoPared());
    }

    IEnumerator CambioSaltoPared()
    {
        saltandoDePared = true;
        yield return new WaitForSeconds(tiempoSaltoPared);
        saltandoDePared = false;
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

        if (life <= 0)
        {
            life = 0;
            Dead();
        }
    }

    private void Dead()
    {
        Time.timeScale = 0;
        Destroy(GetComponent<Player>(), 1);
    }

    private void OnDrawGizmos()
    {
        if (floorController != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(floorController.position, boxDimensions);
            Gizmos.DrawWireCube(controladorPared.position, dimensionCajaPared);
        }
    }
}

