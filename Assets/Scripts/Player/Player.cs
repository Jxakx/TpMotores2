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
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.position += controller.GetMoveDir() * speed * Time.deltaTime;

        isGrounded = Physics2D.OverlapBox(floorController.position, boxDimensions, 0f, floor);

        if (controller.IsJumping() && isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Move(speed * Time.fixedDeltaTime);
    }
    private void Move(float move)
    {
        if(move > 0 && !lookRight)
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
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }
}
