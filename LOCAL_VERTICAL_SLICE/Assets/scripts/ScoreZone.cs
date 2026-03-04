using System.Collections.Generic;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    public int pointsPerRam = 10;
    public int doublePointsPerRam = 20;

    void OnTriggerEnter(Collider other)
    {
        // Normal RAM
        HoldableObject ram = other.GetComponent<HoldableObject>();
        if (ram != null)
        {
            GameManager.Instance.AddScore(pointsPerRam);
            Destroy(other.gameObject);
            return;
        }

        // Heavy RAM
        TwoPlayerHoldable heavy = other.GetComponent<TwoPlayerHoldable>();
        if (heavy != null)
        {
            GameManager.Instance.AddScore(pointsPerRam * 3); // heavy worth more?

            // Force release ALL holders before destroy
            foreach (PlayerInteraction p in new List<PlayerInteraction>(heavy.holders))
            {
                heavy.RemoveHolder(p);
            }

            Destroy(other.gameObject);
        }
    }
}