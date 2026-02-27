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
            col.enabled = false;
        }
    }

    public void RemoveHolder(PlayerInteraction player)
    {
        if (holders.Contains(player))
            holders.Remove(player);

        if (!IsFullyHeld)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = false;
            col.enabled = true;
        }
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

        float alignment = Vector3.Dot(dir1, dir2);

        if (alignment > 0.4f)
        {
            Vector3 moveDir = (dir1 + dir2) / 2f;

            rb.MovePosition(rb.position + moveDir.normalized * 4f * Time.deltaTime);
        }

        // LOCK players to box sides
        Vector3 leftOffset = -transform.right * 1.2f;
        Vector3 rightOffset = transform.right * 1.2f;

        Rigidbody rb1 = p1.GetComponent<Rigidbody>();
        Rigidbody rb2 = p2.GetComponent<Rigidbody>();

        rb1.MovePosition(rb.position + leftOffset);
        rb2.MovePosition(rb.position + rightOffset);
    }
}