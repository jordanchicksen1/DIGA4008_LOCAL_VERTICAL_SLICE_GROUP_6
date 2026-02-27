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
        //picks up HoldableObject (puts it at the holdPoint) as long as the player is holding the pickup button in, and turns off physics
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
        //drops HoldableObject if the player is no longer holding in the pickup button, and turns physics back on
        if (!isHeld) return;

        isHeld = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;

        col.enabled = true;
    }

    public void Throw(Vector3 dir, float force)
    {
        //let's go of HoldableObject and shoots it
        Drop();
        rb.AddForce(dir * force, ForceMode.Impulse);
    }

    public void MoveTowards(Transform target, float speed)
    {
        //move this object towards the player, like a magnet
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );
    }
}
