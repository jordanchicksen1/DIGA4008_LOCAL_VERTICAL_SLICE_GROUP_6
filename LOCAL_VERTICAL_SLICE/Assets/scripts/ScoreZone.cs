using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    public int pointsPerRam = 10;
    public int doublePointsPerRam = 20;

    void OnTriggerEnter(Collider other)
    {
        HoldableObject ram = other.GetComponent<HoldableObject>();
        if (ram != null)
        {
            GameManager.Instance.AddScore(pointsPerRam);
            Destroy(other.gameObject);
        }

        TwoPlayerHoldable biggerRam = other.GetComponent<TwoPlayerHoldable>();
        if (biggerRam != null)
        {
            GameManager.Instance.AddScore(doublePointsPerRam);
            Destroy(other.gameObject);
        }
    }
}