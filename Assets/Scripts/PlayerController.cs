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

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
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
        }

        // Jumping with Coyote Time
        if ((Input.GetKeyDown(KeyCode.Space) && !hasJumped) && (isGrounded || Time.time - lastGroundedTime <= coyoteTime))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            hasJumped = true;
        }
    }
}
