using UnityEngine;
using System.Collections.Generic;

public class TwoPlayerHoldable : MonoBehaviour
{
    public List<PlayerInteraction> holders = new List<PlayerInteraction>();

    Rigidbody rb;
    Collider col;

    public bool IsFullyHeld => holders.Count >= 2;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void AddHolder(PlayerInteraction player)
    {
        if (!holders.Contains(player))
            holders.Add(player);

        if (IsFullyHeld)
        {
            rb.isKinematic = true;

            foreach (PlayerInteraction p in holders)
            {
                Rigidbody prb = p.GetComponent<Rigidbody>();
                prb.isKinematic = true;

                Collider playerCol = p.GetComponent<Collider>();
                Physics.IgnoreCollision(playerCol, col, true);
            }
        }
    }

    public void RemoveHolder(PlayerInteraction player)
    {
        if (!holders.Contains(player))
            return;

        // If this object was fully held before release
        if (IsFullyHeld)
        {
            // Restore physics for both players
            foreach (PlayerInteraction p in holders)
            {
                Rigidbody prb = p.GetComponent<Rigidbody>();
                prb.isKinematic = false;

                Collider playerCol = p.GetComponent<Collider>();
                Physics.IgnoreCollision(playerCol, col, false);
            }

            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        holders.Remove(player);
    }

    void Update()
    {
        if (!IsFullyHeld) return;

        PlayerInteraction p1 = holders[0];
        PlayerInteraction p2 = holders[1];

        PlayerController3D c1 = p1.GetComponent<PlayerController3D>();
        PlayerController3D c2 = p2.GetComponent<PlayerController3D>();

        Vector3 dir1 = c1.CurrentMoveDirection;
        Vector3 dir2 = c2.CurrentMoveDirection;

        if (dir1.magnitude < 0.1f || dir2.magnitude < 0.1f)
            return;

        float alignment = Vector3.Dot(dir1.normalized, dir2.normalized);

        if (alignment > 0.8f)
        {
            Vector3 moveDir = (dir1 + dir2).normalized;
            transform.position += moveDir * 3f * Time.deltaTime;
        }

        Vector3 leftOffset = -transform.right * 1.2f;
        Vector3 rightOffset = transform.right * 1.2f;

        p1.transform.position = transform.position + leftOffset;
        p2.transform.position = transform.position + rightOffset;
    }
}