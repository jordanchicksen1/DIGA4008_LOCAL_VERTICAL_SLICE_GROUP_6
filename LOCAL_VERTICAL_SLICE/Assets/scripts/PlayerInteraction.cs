using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public Transform holdPoint;
    public float pickupRange = 2f;
    public float throwForce = 10f;

    HoldableObject held;
    bool interactHeld;
    bool aiming;

    


    public void OnInteract(InputAction.CallbackContext context)
    {
        //if you aren't holding anything, will try to pick up what is in front of you (accesses HoldableObject)
        if (context.started)
        {
            if (held == null)
            {
                TryPickup();
            }
        }

        //if you let go of the button, will drop the item (accesses Holdable Object)
        if (context.canceled)
        {
            if (held != null)
            {
                held.Drop();
                held = null;
            }
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        //aiming mode turned on if you hold in button
        aiming = context.ReadValueAsButton();
        Debug.Log("Aim: " + aiming);
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        //if you are holding the aiming button and then press the shoot button, it will throw the object (accesses HoldableObject)
        if (context.performed && held != null && aiming)
        {
            Debug.Log("Throw pressed");
            Vector3 dir = transform.forward + Vector3.up * 0.5f;
            held.Throw(dir.normalized, throwForce);
            held = null;
        }
    }


    void TryPickup()
    {
        //looks for HoldableObject within a pickup range. if there is something there, will pick up the object (accesses HoldableObject)
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange);

        foreach (var hit in hits)
        {
            HoldableObject obj = hit.GetComponent<HoldableObject>();
            if (obj != null && !obj.isHeld)
            {
                held = obj;
                obj.PickUp(holdPoint);
                break;
            }
        }
    }
}
