using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    public GameObject batteryPrefab;
    public float respawnTime = 30f;

    GameObject currentBattery;

    void Start()
    {
        SpawnBattery();
    }

    void Update()
    {
        //if player has picked up a battery from that spawner, then another should spawn after 30 secs
        if (currentBattery == null)
        {
            respawnTime -= Time.deltaTime;

            if (respawnTime <= 0)
            {
                SpawnBattery();
                respawnTime = 30f;
            }
        }
    }

    void SpawnBattery()
    {
        currentBattery = Instantiate(batteryPrefab, transform.position, Quaternion.identity);
    }
}