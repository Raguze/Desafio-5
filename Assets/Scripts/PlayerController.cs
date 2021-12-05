using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    public Vector3 CurrentVelocity { get; protected set; }

    public float Horizontal { get; protected set; }
    public bool JumpInput { get; protected set; }
    public bool FloatInput { get; protected set; }
    public bool RunInput { get; protected set; }
    public bool GrabInput { get; protected set; }

    public float Speed = 10f;
    public float JumpSpeed = 10f;

    #region RUN_DASH
    
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    void Update()
    {
        GetInputs();
        GetRigidbodyInfo();

        Jump();
        DoubleJump();
        WallJump();
        Float();
        Walk();
        Run();
        Dash();
        WallClimb();
        LadderClimb();
        PushPull();

        ApplyToRigidbody();

        ResetInputs();
    }

    void GetInputs()
    {
        Horizontal = Input.GetAxis("Horizontal");
        JumpInput = Input.GetKeyDown(KeyCode.Space);
        FloatInput = Input.GetKey(KeyCode.Space);
        RunInput = Input.GetKey(KeyCode.LeftShift);
        
        // Dash
        //KeyCode.LeftControl
        
        // Push Pull
        //KeyCode.F
        
        
    }

    void ResetInputs()
    {
        //JumpInput = false;
        //RunInput = false;
    }

    void GetRigidbodyInfo()
    {
        CurrentVelocity = rb.velocity;
    }

    void ApplyToRigidbody()
    {
        rb.velocity = CurrentVelocity;
    }

    void Jump()
    {
        if(JumpInput)
        {
            CurrentVelocity = CurrentVelocity + Vector3.up * JumpSpeed;
        }
    }

    void DoubleJump()
    {

    }

    void WallJump()
    {

    }

    void Float()
    {
        if(FloatInput)
        {
            if(CurrentVelocity.y < -1) CurrentVelocity = new Vector3(CurrentVelocity.x, -1, 0);
        }
    }

    void Walk()
    {
        CurrentVelocity = new Vector3(Horizontal * Speed, CurrentVelocity.y, 0);
    }

    void Run()
    {
        if(RunInput)
        {
            CurrentVelocity = CurrentVelocity + (Vector3.right * Horizontal * Speed);
        }
    }

    void Dash()
    {

    }

    void WallClimb()
    {

    }

    void LadderClimb()
    {

    }

    void PushPull()
    {

    }
}
