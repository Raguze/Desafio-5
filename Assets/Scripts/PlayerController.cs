using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;

    public float Horizontal { get; protected set; }

    public float Speed = 10f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Horizontal = Input.GetAxis("Horizontal");

        characterController.Move(new Vector3(Horizontal * Speed, 0, 0) * Time.deltaTime);

    }
}
