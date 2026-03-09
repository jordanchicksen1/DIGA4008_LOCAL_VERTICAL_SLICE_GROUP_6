using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    public Transform point;
    public Transform pointPlayer;

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Box")
        {
            other.gameObject.transform.position = point.transform.position;
        }

        if (other.tag == "Player")
        {
            other.gameObject.transform.position = pointPlayer.transform.position;
        }
    }
}
