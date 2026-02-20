using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController3D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float sprintSpeed = 10f;
    public float acceleration = 12f;
    public float rotationSpeed = 15f;

    [Header("Jump")]
    public float jumpForce = 7f;
    public Transform groundCheck;
    public float groundRadius = 0.3f;
    public LayerMask groundLayer;

    Rigidbody rb;
    Vector2 moveInput;
    bool jumpQueued;
    bool sprintHeld;

    float currentSpeed;
    Camera cam;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        currentSpeed = moveSpeed;
    }

    //in fixed update for better physics
    void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
    }

    
    //need to assign these in the player input
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump pressed");
        if (context.performed)
            jumpQueued = true;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        sprintHeld = context.ReadValueAsButton();
    }

    
    //actual methods
    void HandleMovement()
    {
        //declared so that movement and player direction is dependant on camera view
        Vector3 forward = cam.transform.forward; 
        Vector3 right = cam.transform.right;

        forward.y = 0;
        right.y = 0;

        Vector3 direction = forward * moveInput.y + right * moveInput.x;

        //configuring player speed with sprint
        float targetSpeed = sprintHeld ? sprintSpeed : moveSpeed; 
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);

        if (direction.magnitude > 0.1f)
        {
            rb.MovePosition(rb.position + direction.normalized * currentSpeed * Time.fixedDeltaTime);

            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    void HandleJump()
    {
        if (jumpQueued && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); 
        }

        jumpQueued = false;
    }

    //ground check
    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);
    }
}
