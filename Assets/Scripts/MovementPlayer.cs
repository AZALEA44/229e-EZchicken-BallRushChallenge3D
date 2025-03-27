using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float jumpCooldown = 0.2f;
    public float airMultiplier = 0.5f;
    private bool readyToJump = true;

    [Header("Jump Settings")]
    public int maxJumps = 2;
    private int jumpsLeft;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight = 1f;
    public LayerMask whatIsGround;
    private bool grounded;

    public Transform cameraTransform; // Reference to the camera

    private float horizontalInput;
    private float verticalInput;
    private Rigidbody rb;

    [Header("Audio")]
    public AudioSource jumpSound;  // Drag an AudioSource here in the Inspector

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false;
        rb.linearDamping = 0f;
        jumpsLeft = maxJumps;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        if (grounded)
            jumpsLeft = maxJumps;

        MyInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private bool isBreakingMovement;

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Jump logic
        if (Input.GetKeyDown(jumpKey) && jumpsLeft > 0)
        {
            Jump();
        }

        // Break movement logic
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isBreakingMovement = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isBreakingMovement = false;
        }
    }

    public float slowDownSpeed = 5f;

    private void MovePlayer()
    {
        if (cameraTransform == null) return;

        if (isBreakingMovement)
        {
            Vector3 targetVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, slowDownSpeed * Time.deltaTime);
            return;
        }

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward = Vector3.ProjectOnPlane(camForward, Vector3.up).normalized;
        camRight = Vector3.ProjectOnPlane(camRight, Vector3.up).normalized;

        Vector3 moveDirection = (camForward * verticalInput + camRight * horizontalInput).normalized;

        if (moveDirection != Vector3.zero)
        {
            rb.AddTorque(new Vector3(moveDirection.z, 0, -moveDirection.x) * moveSpeed, ForceMode.Force);
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpsLeft--;

        if (jumpSound != null)
        {
            jumpSound.Play();  // Play the jump sound
        }
    }
}
