using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public void Break()
    {
        // space for vfx or something

        //then destroy the wall
        Destroy(gameObject);
    }
}