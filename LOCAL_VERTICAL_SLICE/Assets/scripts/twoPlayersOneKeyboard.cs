using UnityEngine;
using UnityEngine.InputSystem;

public class twoPlayersOneKeyboard : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] private InputActionReference p1Move;
    [SerializeField] private InputActionReference p2Move;

    [Header("Player")]
    [SerializeField] private Transform p1;
    [SerializeField] private Transform p2;

    [SerializeField] private float speed = 5f;
    private void OnEnable()
    {
        p1Move.action.Enable();
        p2Move.action.Enable();
    }

    private void OnDisable()
    {
        p1Move.action.Disable();
        p2Move.action.Disable();
    }

    private void Update()
    {
        var m1 = p1Move.action.ReadValue<Vector2>();
        var m2 = p2Move.action.ReadValue<Vector2>();

        if (p1) p1.position += new Vector3(m1.x, 0f, m1.y) * speed * Time.deltaTime;
        if (p2) p2.position += new Vector3(m2.x, 0f, m2.y) * speed * Time.deltaTime;
    }
}
