using UnityEngine;

public class PlayerOne : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpStep = 1f;
    [SerializeField] private float rotateSpeed = 100f;
    private Vector2 lookInput;
    private Vector2 moveInput;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        float yaw = lookInput.x * rotateSpeed * Time.deltaTime;
        transform.Rotate(0f, yaw, 0f, Space.World);

        Vector3 move3 = new Vector3(moveInput.x, moveInput.y, 0f) * moveSpeed * Time.deltaTime;
    }
}
