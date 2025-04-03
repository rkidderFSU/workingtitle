using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float movementSpeed;
    [SerializeField] private float jumpForce = 10f;
    private bool isGrounded;
    private Animator animator;

    float horizontalInput;
    float verticalInput;

    bool canMove;
    bool blocking;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (canMove)
        {
            Movement();
        }
        CombatFunction();
    }

    void Movement()
    {
        // Get the current velocity
        Vector3 currentVelocity = rb.velocity;
        // Calculate movement direction based on input
        Vector3 movement = (transform.forward * verticalInput + transform.right * horizontalInput) * movementSpeed;
        // Preserve Y-axis velocity so gravity and jumping still work properly
        rb.velocity = new Vector3(movement.x, currentVelocity.y, movement.z);

        // Update Animator Parameters
        animator.SetFloat("moveSpeed", movement.magnitude);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("Jump", true);
        }
    }

    void CombatFunction()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Trigger block animation
            animator.SetBool("Blocking", true);
            blocking = true;
            canMove = false; // Disables movement while blocking
            Debug.Log("block");
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            // Stop block animation
            animator.SetBool("Blocking", false);
            blocking = false;
            canMove = true; // Re-enables movement
            Debug.Log("not block");
        }
        if (Input.GetMouseButtonDown(0) && !blocking) // Left click
        {
            // Trigger punch animation
            animator.SetTrigger("Punch");
            Debug.Log("punch");
        }
        if (Input.GetMouseButtonDown(1) && !blocking) // Right click
        {
            // Trigger kick animation
            animator.SetTrigger("Kick");
            Debug.Log("kick");
        }
    }

    // Detect Ground
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("Jump", false);
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
