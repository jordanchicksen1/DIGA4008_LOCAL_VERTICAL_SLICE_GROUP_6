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

    public float magnetSpeed = 10f;
    HoldableObject pullingObject;

    TwoPlayerHoldable heavyHeld;

    PlayerInput playerInput;
    bool isPlayerOne;

    public bool IsCarryingHeavy => heavyHeld != null && heavyHeld.IsFullyHeld;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        isPlayerOne = playerInput.playerIndex == 0;
    }
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

        //if you let go of the button, will drop the item (accesses Holdable Object or Two Player Holdable)
        if (context.canceled)
        {
            if (held != null)
            {
                held.Drop();
                held = null;
            }

            if (heavyHeld != null)
            {
                heavyHeld.RemoveHolder(this);
                heavyHeld = null;
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
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange);

        foreach (var hit in hits)
        {
            // 1️⃣ Heavy objects stay the same
            TwoPlayerHoldable heavy = hit.GetComponent<TwoPlayerHoldable>();
            if (heavy != null)
            {
                heavy.AddHolder(this);
                heavyHeld = heavy;
                return;
            }

            // 2️⃣ Normal holdable objects
            HoldableObject obj = hit.GetComponent<HoldableObject>();
            if (obj != null && !obj.isHeld)
            {
                if (isPlayerOne)
                {
                    // Player 1 pulls it
                    pullingObject = obj;
                }
                else
                {
                    // Player 2 pushes it away
                    Rigidbody rb = obj.GetComponent<Rigidbody>();

                    Vector3 repelDirection =
                        (obj.transform.position - transform.position).normalized;

                    rb.AddForce(repelDirection * magnetSpeed, ForceMode.Impulse);
                }

                return;
            }
        }
    }

    void Update()
    {
        if (isPlayerOne && pullingObject != null)
        {
            Vector3 direction =
                (holdPoint.position - pullingObject.transform.position).normalized;

            pullingObject.transform.position +=
                direction * magnetSpeed * Time.deltaTime;

            float distance = Vector3.Distance(
                pullingObject.transform.position,
                holdPoint.position
            );

            if (distance < 0.2f)
            {
                held = pullingObject;
                held.PickUp(holdPoint);
                pullingObject = null;
            }
        }
    }

    public bool IsHoldingHeavy()
    {
        return heavyHeld != null;
    }
}
