using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;

public class PlayerInteraction : MonoBehaviour
{
    private MusicManager MusicManager;
    public Transform holdPoint;
    public float pickupRange = 2f;
    public float throwForce = 10f;
    public bool Holding;

    HoldableObject held;
    bool interactHeld;
    bool aiming;

    public float magnetSpeed = 10f;
    HoldableObject pullingObject;

    TwoPlayerHoldable heavyHeld;

    PlayerInput playerInput;

    private Gamepad Gamepad;
    public bool Rumble;
    public float timz;


    
    public bool IsCarryingHeavy => heavyHeld != null && heavyHeld.IsFullyHeld;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        //MusicManager = GetComponent<MusicManager>();
        MusicManager = FindAnyObjectByType<MusicManager>();
        
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        //if you aren't holding anything, will try to pick up what is in front of you (accesses HoldableObject)
        if (context.started)
        {
            if (held == null)
            {
                TryPickup();
                //Debug.Log("Pickup");

                //maybe need to use some coruitines 
                /*var device = context.control.device;
                
                if(device is Gamepad gamepad) 
                {
                    Gamepad = gamepad;  

                        if (gamepad is DualShockGamepad)
                        {
                            Debug.Log(gamepad.displayName);
                            //gamepad.SetMotorSpeeds(0.5f, 0.5f);
                            StartCoroutine(DualShockRumble());
                        }

                        if (gamepad is DualSenseGamepad)
                        {
                            Debug.Log(gamepad.displayName);
                            //gamepad.SetMotorSpeeds(0.4f, 0.7f);
                            StartCoroutine(DualSenseRumble());
                        }

                        if (gamepad is XInputController)
                        {
                            Debug.Log(gamepad.displayName);
                            //gamepad.SetMotorSpeeds(0.3f, 0.7f);
                            StartCoroutine(XboxRumble());
                        }
                }*/

              
            }
        }

        //if you let go of the button, will drop the item (accesses Holdable Object or Two Player Holdable)
        if (context.canceled)
        {
            if (held != null)
            {
                held.Drop();
                held = null;
                //Debug.Log("Drop");
                //Holding = false;
                Gamepad.SetMotorSpeeds(0.0f, 0.0f);
                //Rumble = false;
                //timz = 0;
            }



            if (heavyHeld != null)
            {
                heavyHeld.RemoveHolder(this);

                // Clear local reference
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
            // Heavy objects stay the same
            TwoPlayerHoldable heavy = hit.GetComponent<TwoPlayerHoldable>();
            if (heavy != null)
            {
                heavy.AddHolder(this);
                heavyHeld = heavy;
                //Holding = true;

                //Maybe make rumble more intense
                Gamepad = Gamepad.current;

                if (Gamepad is Gamepad gamepad)
                {
                    Gamepad = gamepad;

                    if (gamepad is DualShockGamepad)
                    {
                        Debug.Log(gamepad.displayName);
                        //gamepad.SetMotorSpeeds(0.5f, 0.5f);
                        StartCoroutine(HeavyDualShockRumble());
                    }

                    if (gamepad is DualSenseGamepad)
                    {
                        Debug.Log(gamepad.displayName);
                        //gamepad.SetMotorSpeeds(0.4f, 0.7f);
                        StartCoroutine(HeavyDualSenseRumble());
                    }

                    if (gamepad is XInputController)
                    {
                        Debug.Log(gamepad.displayName);
                        //gamepad.SetMotorSpeeds(0.3f, 0.7f);
                        StartCoroutine(HeavyXboxRumble());
                    }
                }

                return;
            }

            // Normal holdable objects
            HoldableObject obj = hit.GetComponent<HoldableObject>();
            if (obj != null && !obj.isHeld)
            {
              
                    pullingObject = obj;
                //Holding = true ;
                MusicManager.SFX.PlayOneShot(MusicManager.Electricity);
                //MusicManager.GetComponent<MusicManager>().SFX.PlayOneShot(MusicManager.Electricity);
                //Debug.Log("hold");


                Gamepad = Gamepad.current;

                if (Gamepad is Gamepad gamepad)
                {
                    Gamepad = gamepad;

                    if (gamepad is DualShockGamepad)
                    {
                        Debug.Log(gamepad.displayName);
                        //gamepad.SetMotorSpeeds(0.5f, 0.5f);
                        StartCoroutine(DualShockRumble());
                    }

                    if (gamepad is DualSenseGamepad)
                    {
                        Debug.Log(gamepad.displayName);
                        //gamepad.SetMotorSpeeds(0.4f, 0.7f);
                        StartCoroutine(DualSenseRumble());
                    }

                    if (gamepad is XInputController)
                    {
                        Debug.Log(gamepad.displayName);
                        //gamepad.SetMotorSpeeds(0.3f, 0.7f);
                        StartCoroutine(XboxRumble());
                    }
                }
                    return;

            }
        }
    }

    void Update()
    {
        
        if (pullingObject != null)
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

    

    public void ClearHeavyReference()
    {
        heavyHeld = null;
    }

    public bool IsHoldingHeavy()
    {
        return heavyHeld != null && heavyHeld.holders.Contains(this);
    }

    public bool HasNormalObject()
    {
        return held != null;
    }

   public IEnumerator DualShockRumble() 
    {
        Gamepad = Gamepad.current;
        Debug.Log(Gamepad.displayName);
        Gamepad.SetMotorSpeeds(0.5f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Gamepad.SetMotorSpeeds(0f, 0f);
    }

    public IEnumerator DualSenseRumble()
    {
        Gamepad = Gamepad.current;
        Debug.Log(Gamepad.displayName);
        Gamepad.SetMotorSpeeds(0.3f, 0.3f);
        yield return new WaitForSeconds(0.5f);
        Gamepad.SetMotorSpeeds(0f, 0f);
    }

    public IEnumerator XboxRumble()
    {
        Gamepad = Gamepad.current;
        Debug.Log(Gamepad.displayName);
        Gamepad.SetMotorSpeeds(0.6f, 0.6f);
        yield return new WaitForSeconds(0.5f);
        Gamepad.SetMotorSpeeds(0f, 0f);
    }

    public IEnumerator HeavyDualShockRumble()
    {
        Gamepad = Gamepad.current;
        Debug.Log(Gamepad.displayName);
        Gamepad.SetMotorSpeeds(0.6f, 0.6f);
        yield return new WaitForSeconds(0.5f);
        Gamepad.SetMotorSpeeds(0f, 0f);
    }

    public IEnumerator HeavyDualSenseRumble()
    {
        Gamepad = Gamepad.current;
        Debug.Log(Gamepad.displayName);
        Gamepad.SetMotorSpeeds(0.4f, 0.4f);
        yield return new WaitForSeconds(0.5f);
        Gamepad.SetMotorSpeeds(0f, 0f);
    }

    public IEnumerator HeavyXboxRumble()
    {
        Gamepad = Gamepad.current;
        Debug.Log(Gamepad.displayName);
        Gamepad.SetMotorSpeeds(0.7f, 0.7f);
        yield return new WaitForSeconds(0.5f);
        Gamepad.SetMotorSpeeds(0f, 0f);
    }
}
