using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    public Light pointLight;
    public float maxLight = 5f;
    public float drainRate = 0.5f;

    float currentLight;

    PlayerInteraction interaction;

    void Start()
    {
        //make sure that light starts out at max
        currentLight = maxLight;
        //fetch holding logic so that light can decrease accordingly
        interaction = GetComponent<PlayerInteraction>();
    }

    void Update()
    {
        // don't drain light until the round actually starts
        if (!GameManager.Instance.gameStarted)
            return;
        
        //drain normal
        float drainMultiplier = 1f;

        if (interaction != null)
        {
            // If holding normal object or heavy object
            if (interaction.IsHoldingHeavy() || interaction.HasNormalObject())
            {
                // drain double
                drainMultiplier = 2f;
            }
        }

        // drain light over time
        currentLight -= drainRate * drainMultiplier * Time.deltaTime;

        // clamp between 0 and max
        currentLight = Mathf.Clamp(currentLight, 0, maxLight);

        // update light intensity
        pointLight.intensity = currentLight;
    }

    public void AddLight(float amount)
    {
        //add to light amount
        currentLight += amount;
        currentLight = Mathf.Clamp(currentLight, 0, maxLight);
    }

    public float GetLightPercent()
    {
        
        return currentLight / maxLight;
    }
}