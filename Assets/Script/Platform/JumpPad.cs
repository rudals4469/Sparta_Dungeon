using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField]private float jumpForce = 15f; // 튕겨나는 힘
    public Vector3 jumpDirection = Vector3.up; // 윗쪽 방향

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(jumpDirection.normalized * jumpForce, ForceMode.Impulse);
            }
        }
    }
}
