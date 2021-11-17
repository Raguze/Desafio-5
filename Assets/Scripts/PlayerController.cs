using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    public float Horizontal { get; protected set; }

    public float Speed = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vector3 velocity = rb.velocity;
        rb.velocity  = new Vector3(Horizontal * Speed, velocity.y, 0);

    }
}
