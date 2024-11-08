using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public int life;
    public int maxLife;
    [SerializeField] Controller controller;
    [SerializeField] float speed = 5;
    [SerializeField] float smoothedMove;
    private bool lookRight = true;
    public GameplayManager gamePlayCanvas;

    [Header("Salto")]
    [SerializeField] LayerMask floor;
    [SerializeField] Transform floorController;
    [SerializeField] Vector3 boxDimensions;
    [SerializeField] float jumpForce = 5f;
    private bool isGrounded = false;

    [Header("Dash")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float dashCooldown = 3f;  
    private float dashCooldownTimer = 0f;
    private bool isDashing = false;
    private float starterGravity;
    private bool canDash = true;
    private bool canMove = true; //Tambi�n ayuda al knowback cuando choca con alg�n enemy

    [Header("Knockback")]
    [SerializeField] public Vector2 knockBackSpeed; //Knockback
    [SerializeField] private float timeLostControl; //Knockback

    [Header("DoubleJump")]
    [SerializeField] private int saltosExtraRestantes;
    [SerializeField] private int saltosExtra;

    [Header("Rebote")]
    [SerializeField] float speedRebound;

    [Header("SaltoPared")]
    [SerializeField] private Transform controladorPared;
    [SerializeField] private Vector3 dimensionCajaPared;
    [SerializeField] private bool enPared;
    [SerializeField] private bool deslizando;
    [SerializeField] private float velocidadDeslizar;
    [SerializeField] private float fuerzaSaltoParedX;
    [SerializeField] private float fuerzaSaltoParedY;
    [SerializeField] private float tiempoSaltoPared;
    private bool saltandoDePared;

    [Header("Animaciones")]
    private Animator animator;

    // Para las part�culas cuando corre/camina/dash
    [SerializeField] private ParticleSystem particulasDash;
    [SerializeField] private ParticleSystem particulasCorrer;
    [SerializeField] private ParticleSystem particulasAterrizaje;
    [SerializeField] private ParticleSystem particulasDj;

    private bool wasGrounded = true; // Para que cuando toque el suelo, aparezcan las part�culas de aterrizaje

    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip doubleJumpSound;
    [SerializeField] private AudioClip dashSound;

    [SerializeField] private BarraDeVida barraDeVida;

    [Header("Checkpoint")]
    [SerializeField] private CheckpointManager checkpointManager;

    private void Start()
    {
        life = maxLife;
        barraDeVida.InicializarBarraDeVida(life);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        starterGravity = rb.gravityScale;
        checkpointManager = FindObjectOfType<CheckpointManager>();
        life = maxLife;
        if (checkpointManager != null)
        {
            checkpointManager.UpdateCheckpointPosition(transform.position);
        }
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapBox(floorController.position, boxDimensions, 0f, floor);
        enPared = Physics2D.OverlapBox(controladorPared.position, dimensionCajaPared, 0f, floor);

        if (!canDash)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer <= 0)
            {
                canDash = true; 
            }
        }

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
            if (controller.GetMoveDir().x != 0) // Solo deslizar si se est� moviendo
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

    public void Knockback(Vector2 punchPoint)
    {
        rb.velocity = new Vector2(-knockBackSpeed.x * punchPoint.x, knockBackSpeed.y);
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
                particulasCorrer.Play();  // Iniciar part�culas cuando corra
            }
        }
        else
        {
            if (particulasCorrer.isPlaying)
            {
                particulasCorrer.Stop();  // Detener part�culas cuando no corra
            }
        }

        if (controller.IsDashing() && canDash && !isDashing)
        {
            StartCoroutine(Dash());
            particulasDash.Play();
            ControllerSFX.instance.executeSound(dashSound);
        }

        // Actualizar par�metros de animaci�n
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
            else if (enPared && deslizando)
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
        isDashing = true;
        canMove = false;
        canDash = false;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(dashSpeed * transform.localScale.x, 0);

        yield return new WaitForSeconds(dashTime);

        canMove = true;
        rb.gravityScale = starterGravity;

        yield return new WaitForSeconds(dashCooldown);
        dashCooldownTimer = dashCooldown;

        isDashing = false;
    }

    public void Rebound()
    {
        rb.velocity = new Vector2(rb.velocity.x, speedRebound);
    } 
    
    public void TakeDamage(int value, Vector2 posicion)
    {
        life -= value;
        barraDeVida.CambiarVidaActual(life);
        animator.SetTrigger("Golpe");
        StartCoroutine(LostControl()); //Perder el control
        StartCoroutine(CollisionDesactive()); //Desactivar Colisiones
        Knockback(posicion);

        if (life <= 0)
        {
            life = 0;
            Dead();
            gamePlayCanvas.Onlose();
        }
    }
    private IEnumerator CollisionDesactive() //Knowback (Invensibilidad por un tiempito)
    {
        Physics2D.IgnoreLayerCollision(6, 8, true); //Desactiva las colisiones del player y los enemigos cuando ocurra el knockback
        yield return new WaitForSeconds(timeLostControl);
        Physics2D.IgnoreLayerCollision(6, 8, false);
    }
    private IEnumerator LostControl() //Knowback
    {
        canMove = false;
        yield return new WaitForSeconds(timeLostControl);
        canMove = true;
    }

    public void ResetPlayerCollisions()
    {
        Physics2D.IgnoreLayerCollision(6, 8, false);  // Aseg�rate de que las colisiones se restauran
    }

    public void Curar(int cantidadCuracion)
    {
        life += cantidadCuracion;

        // Limitar la vida para que no sobrepase el m�ximo
        if (life > maxLife)
        {
            life = maxLife;
        }

        // Actualizar la barra de vida
        barraDeVida.CambiarVidaActual(life);
    }

    private void Dead()
    {
        Time.timeScale = 0;
        //Destroy(GetComponent<Player>(), 1);
    }


    public void RespawnAtCheckpoint()
    {
        if (checkpointManager != null)
        {
            // Desactiva moment�neamente el Rigidbody para evitar efectos residuales
            rb.velocity = Vector2.zero;
            transform.position = checkpointManager.GetCheckpointPosition();
            life = maxLife;
            barraDeVida.InicializarBarraDeVida(life);
            Debug.Log("Jugador respawneado en checkpoint: " + transform.position);
        }
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

