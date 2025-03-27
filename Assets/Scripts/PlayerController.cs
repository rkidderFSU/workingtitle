using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float movementSpeed;
    [SerializeField] private float coyoteTime = 0.2f;
    private float lastGroundedTime;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer; // Assign this in the Inspector
    [SerializeField] private Transform groundCheck; // Empty GameObject positioned at the player's feet
    [SerializeField] private float groundCheckRadius = 0.2f;
    private bool isGrounded;
    private bool hasJumped;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if ((horizontalInput > 0.1 || horizontalInput < -0.1 || verticalInput > 0.1 || verticalInput < -0.1) && !animator.GetBool("Jump"))
        {
            animator.SetBool("Walking", true);
        } else
        {
            animator.SetBool("Walking", false);

        }

        // Get the current velocity
        Vector3 currentVelocity = rb.velocity;


        // Calculate movement direction based on input
        Vector3 movement = (transform.forward * verticalInput + transform.right * horizontalInput) * movementSpeed;
        
        // Preserve Y-axis velocity so gravity and jumping still work properly
        rb.velocity = new Vector3(movement.x, currentVelocity.y, movement.z);

        // Ground check using Physics.CheckSphere
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // Update last grounded time
        if (isGrounded)
        {
            lastGroundedTime = Time.time;
            hasJumped = false;
            animator.SetBool("Jump", false);
        }
        else
        {
            hasJumped = true;
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || Time.time - lastGroundedTime <= coyoteTime) && !hasJumped)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            hasJumped = true;
            animator.Play("Jump");
        }

        if (Time.time - lastGroundedTime <= coyoteTime && hasJumped)
        {
            isGrounded = false;
        }
    }
}
