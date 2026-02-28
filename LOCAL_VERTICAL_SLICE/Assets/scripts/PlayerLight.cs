using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    public Light pointLight;
    public float maxLight = 5f;
    public float drainRate = 0.5f;

    float currentLight;

    void Start()
    {
        currentLight = maxLight;
    }

    void Update()
    {
        currentLight -= drainRate * Time.deltaTime;
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