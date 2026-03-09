using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    public Transform point;

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Box")
        {
            other.gameObject.transform.position = point.transform.position;
        }
    }
}
