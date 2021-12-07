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
    public float climbingSpeed = 3f;
    public LayerMask groundLayer;

    public float MovableBoxDetectorDistance = 0.75f;

    public LayerMask MovableBoxMask;

    GameObject MovableBox;

    #region RUN_DASH
    
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        MovableBoxMask = LayerMask.GetMask("Movable Box");
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
        GrabInput = Input.GetKeyDown(KeyCode.F);
        
        
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
        if (Physics.Raycast(this.transform.position - new Vector3(0, 0.9f, 0), -this.transform.right, 0.6f) && Horizontal < 0)
        {
            rb.useGravity = false;
            transform.position += Vector3.up * climbingSpeed * Time.deltaTime;
        }
        else if (Physics.Raycast(this.transform.position - new Vector3(0, 0.9f,0), this.transform.right, 0.6f) && Horizontal > 0)
        {
            rb.useGravity = false;
            transform.position += Vector3.up * climbingSpeed * Time.deltaTime;
        }
        else
        {
            rb.useGravity = true;
        }
    }

    void LadderClimb()
    {

    }

    void PushPull()
    {
        bool HitR = Physics.Raycast(transform.position,Vector2.right*transform.localScale.x,MovableBoxDetectorDistance, MovableBoxMask);
        bool HitL = Physics.Raycast(transform.position,Vector2.right*transform.localScale.x*-1,MovableBoxDetectorDistance, MovableBoxMask);
        if(HitL == false && HitR == false){
            MovableBox = null;
            Speed = 10f;
        }
        if (HitR && GrabInput || HitL && GrabInput){
            Debug.Log("Puxei");
            MovableBox.GetComponent<MeshRenderer>().material.color = Color.blue;
            FixedJoint FJ = MovableBox.AddComponent<FixedJoint>();
            FJ.connectedBody = this.rb;
            FJ.breakForce = 50f;
            Speed = 1.5f;
        }
        if(Input.GetKeyUp(KeyCode.F)){
            MovableBox.GetComponent<MeshRenderer>().material.color = Color.white;
            FixedJoint FJ = MovableBox.GetComponent<FixedJoint>();
            Destroy(FJ);
            
            
            Speed = 10f;
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "Movable Box"){
            MovableBox = collisionInfo.gameObject;
        }
    }
}
