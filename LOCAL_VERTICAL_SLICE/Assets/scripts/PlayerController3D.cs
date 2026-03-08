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
    PlayerLight playerLight;
    public Vector2 RawMoveInput => moveInput;
    public Vector3 CurrentMoveDirection { get; private set; }

    [Header("Jump")]
    public float jumpForce = 7f;
    public Transform groundCheck;
    public float groundRadius = 0.3f;
    public LayerMask groundLayer;

    [Header("Anim")]
    public Animator animator;
    public PlayerInteraction PlayerInteraction;

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
        playerLight = GetComponent<PlayerLight>();
    }

    //in fixed update for better physics
    void FixedUpdate()
{
    if (IsCarryingHeavy())
        return;

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
        
        if (context.performed)
            jumpQueued = true;
            Debug.Log("Jump pressed");
        //animator.SetBool("Jump", true);
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
        CurrentMoveDirection = direction.normalized;

        //configuring player speed with sprint
        float baseSpeed = sprintHeld ? sprintSpeed : moveSpeed;

        float batteryPercent = 1f;

        if (playerLight != null)
        {
            batteryPercent = playerLight.GetLightPercent();
        }

        if(moveInput.y > 0 || moveInput.x > 0 || moveInput.y < 0 || moveInput.x < 0) 
        {
            //Debug.Log("moving");
            animator.SetBool("Walk", true); 

           
        }

        /*if (moveInput.y > 0 || moveInput.x > 0 || moveInput.y < 0 || moveInput.x < 0 && PlayerInteraction.Holding == true) 
        {
            Debug.Log("Hold and Walk");
            //animator.SetBool("Walk", false);
            animator.SetBool("HoldWalk", true);
        }*/

            if (moveInput.y == 0 && moveInput.x == 0 ) 
        {
                animator.SetBool("Walk", false);
        }
/*
        if (PlayerInteraction.Holding == true) 
        {
            Debug.Log("hold");
            animator.SetBool("HoldWalk", true);
        }

        if (PlayerInteraction.Holding == false)
        {
            Debug.Log("not hold");
            animator.SetBool("HoldWalk", false);
        }*/



        // Speed scales from 10% ? 100%
        float speedMultiplier = Mathf.Lerp(0.25f, 1f, batteryPercent);

        float targetSpeed = baseSpeed * speedMultiplier;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);

        if (direction.magnitude > 0.1f)
        {
            rb.MovePosition(rb.position + direction.normalized * currentSpeed * Time.fixedDeltaTime);

            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
            animator.SetBool("Run", true);
        }

        if(direction.magnitude < 0.1f) 
        {
            animator.SetBool("Run", false);
        }
    }

    void HandleJump()
    {
        //if the player can jump, then shoot them in the air
        if (jumpQueued && IsGrounded() && !IsCarryingHeavy())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //animator.SetBool("Jump", false);
        }

        jumpQueued = false;

      
    }

    //ground check
    bool IsGrounded()
    {
        //animator.SetBool("Jump", true);
        //Debug.Log("Grounded");
        return Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);
        
    }

    void OnCollisionEnter(Collision collision)
    {
        //breakable wall - if you are moving fast enough, fetch and activate the break method
        if (currentSpeed < sprintSpeed * 0.5f) return;

        BreakableWall wall = collision.gameObject.GetComponent<BreakableWall>();

        if (wall != null)
        {
            wall.Break();
        }
    }

    public bool IsCarryingHeavy()
    {
        PlayerInteraction interaction = GetComponent<PlayerInteraction>();
        return interaction != null && interaction.IsHoldingHeavy();
    }
}
