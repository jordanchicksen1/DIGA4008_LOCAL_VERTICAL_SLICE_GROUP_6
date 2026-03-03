using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    public float rechargeAmount = 2f;

    void OnTriggerEnter(Collider other)
    {
        PlayerLight light = other.GetComponent<PlayerLight>();
        if (light != null)
        {
            light.AddLight(rechargeAmount);
            Destroy(gameObject);
        }
    }
}