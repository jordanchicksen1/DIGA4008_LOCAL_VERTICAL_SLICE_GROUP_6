using UnityEngine;

public class HoldableObject : MonoBehaviour
{
    Rigidbody rb;
    Collider col;

    public bool isHeld { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void PickUp(Transform holdPoint)
    {
        if (isHeld) return;

        isHeld = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;
        rb.useGravity = false;

        col.enabled = false;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        if (!isHeld) return;

        isHeld = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;

        col.enabled = true;
    }

    public void Throw(Vector3 dir, float force)
    {
        Drop();
        rb.AddForce(dir * force, ForceMode.Impulse);
    }
}
