using UnityEngine;
using UnityEngine.InputSystem;

public class twoPlayersOneKeyboard : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] private InputActionReference p1Move;
    [SerializeField] private InputActionReference p2Move;
    [SerializeField] private InputActionReference p1Jump;
    [SerializeField] private InputActionReference p2Jump;

    [Header("Player")]
    [SerializeField] private Transform p1;
    [SerializeField] private Transform p2;
    [SerializeField] private Rigidbody p1RB;
    [SerializeField] private Rigidbody p2RB;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 5f;

    private void OnEnable()
    {
        p1Move.action.Enable();
        p2Move.action.Enable();
        p1Jump.action.Enable();
        p1Jump.action.performed += OnJumpP1;
        p2Jump.action.Enable();
        p2Jump.action.performed += OnJumpP2;
    }

    private void OnDisable()
    {
        p1Move.action.Disable();
        p2Move.action.Disable();
        p1Jump.action.Disable();
        p1Jump.action.performed -= OnJumpP1;
        p2Jump.action.Disable();
        p2Jump.action.performed -= OnJumpP2;
    }

    private void Update()
    {
        var m1 = p1Move.action.ReadValue<Vector2>();
        var m2 = p2Move.action.ReadValue<Vector2>();

        if (p1) p1.position += new Vector3(m1.x, 0f, m1.y) * speed * Time.deltaTime;
        if (p2) p2.position += new Vector3(m2.x, 0f, m2.y) * speed * Time.deltaTime;

    }

    public void OnJumpP1(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        p1RB.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        Debug.Log("pressed jump p1");
    }

    public void OnJumpP2(InputAction.CallbackContext context) 
    {
        if (!context.performed) return;

        p2RB.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        Debug.Log("pressed jump p2");
    }

    
}
