using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    Camera cam;

    void Awake()
    {
        //get camera
        cam = FindFirstObjectByType<Camera>();
    }

    void LateUpdate()
    {
        if (!cam) return;
        
        Vector3 direction = transform.position - cam.transform.position;
        direction.y = 0f; // lock vertical tilt

        //make sure that ui is always facing the camera
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
