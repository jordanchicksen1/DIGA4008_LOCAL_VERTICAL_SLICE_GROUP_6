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
        currentLight = maxLight;
        interaction = GetComponent<PlayerInteraction>();
    }

    void Update()
    {
        float drainMultiplier = 1f;

        if (interaction != null)
        {
            // If holding normal object OR heavy object
            if (interaction.IsHoldingHeavy() || interaction.HasNormalObject())
            {
                drainMultiplier = 2f;
            }
        }

        currentLight -= drainRate * drainMultiplier * Time.deltaTime;
        currentLight = Mathf.Clamp(currentLight, 0, maxLight);

        pointLight.intensity = currentLight;
    }

    public void AddLight(float amount)
    {
        currentLight += amount;
        currentLight = Mathf.Clamp(currentLight, 0, maxLight);
    }

    public float GetLightPercent()
    {
        return currentLight / maxLight;
    }
}