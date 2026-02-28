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
        if (holders.Count >= 2)
            return;

        

        if (!holders.Contains(player))
            holders.Add(player);

        if (IsFullyHeld)
        {
            rb.isKinematic = true;

            foreach (PlayerInteraction p in holders)
            {
                Rigidbody prb = p.GetComponent<Rigidbody>();
                prb.isKinematic = true;

                
            }
        }
    }

    public void RemoveHolder(PlayerInteraction player)
    {
        if (!holders.Contains(player))
            return;

        holders.Remove(player);

        // Restore releasing player's physics
        Rigidbody releasingRb = player.GetComponent<Rigidbody>();
        releasingRb.isKinematic = false;
        releasingRb.linearVelocity = Vector3.zero;
        releasingRb.angularVelocity = Vector3.zero;

        player.ClearHeavyReference();

        if (holders.Count < 2)
        {
            // Restore box physics
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Restore remaining player (if any)
            foreach (PlayerInteraction p in holders)
            {
                Rigidbody prb = p.GetComponent<Rigidbody>();
                prb.isKinematic = false;
            }
        }
    }

    void Update()
    {
        if (!IsFullyHeld)
            return;

        PlayerInteraction p1 = holders[0];
        PlayerInteraction p2 = holders[1];

        PlayerController3D c1 = p1.GetComponent<PlayerController3D>();
        PlayerController3D c2 = p2.GetComponent<PlayerController3D>();

        //player 1 controls forward + back
        Vector2 moveInput = c1.RawMoveInput;
        float moveAmount = moveInput.y; 

        transform.position += transform.forward * moveAmount * 4f * Time.deltaTime;

        //player 2 controls turning
        float rotationAmount = c2.RawMoveInput.x * 220f * Time.deltaTime;
        transform.Rotate(Vector3.up, rotationAmount);

        //match box height to players
        float averageY = (p1.transform.position.y + p2.transform.position.y) / 2f;

        Vector3 boxPos = transform.position;
        boxPos.y = averageY;
        transform.position = boxPos;

        //lock players to sides (preserve their Y)
        Vector3 leftOffset = -transform.right * 1.2f;
        Vector3 rightOffset = transform.right * 1.2f;

        Vector3 p1Pos = transform.position + leftOffset;
        Vector3 p2Pos = transform.position + rightOffset;

        p1Pos.y = p1.transform.position.y;
        p2Pos.y = p2.transform.position.y;

        p1.transform.position = p1Pos;
        p2.transform.position = p2Pos;

        p1.transform.rotation = transform.rotation;
        p2.transform.rotation = transform.rotation;
    }
}