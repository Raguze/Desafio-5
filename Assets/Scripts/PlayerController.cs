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
    private float wallStickTime = 0.5f;
    private float timeToUnstick = 0.0f;
    public LayerMask groundLayer;

    public float MovableBoxDetectorDistance = 0.75f;

    public LayerMask MovableBoxMask;

    GameObject MovableBox;

    #region RUN_DASH
    
    #endregion

    #region COLLISION_INFO
    private bool lookingRight = true;
    private bool onTheGround = false;
    private bool collidingLeft = false;
    private bool collidingRight = false;
    private bool onTheWall = false;
    private bool collidingUp = false;

    
    private Vector3 rayPos = new Vector3();

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        MovableBoxMask = LayerMask.GetMask("Movable Box");

        CalculateRayPos();
    }



    void Update()
    {
        GetInputs();
        GetRigidbodyInfo();
        GetCollisionInfo();

// Debug.Log(collidingRight + " " + onTheWall);

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
        ResetCollisions();

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

    void CalculateRayPos()
    {
        CapsuleCollider cC = GetComponent<CapsuleCollider>();
        rayPos = new Vector3(cC.radius-0.02f, cC.height-0.02f);
    }

    void GetCollisionInfo()
    {       
        if(Horizontal != 0)
            lookingRight = Horizontal > 0;

        Vector3 direction = lookingRight ? Vector3.right : Vector3.left;
        Vector3 origin = this.transform.position + new Vector3(direction.x*rayPos.x , -rayPos.y); 
        float length =  Horizontal * Speed * Time.fixedDeltaTime;

        if(Horizontal == 0)
            length += 0.02f;
        
        for(int i = 0; i < 3; i++)
        {
            //Debug.DrawRay(origin + i*rayPos.y*Vector3.up, direction*length, Color.blue);

            if(Physics.Raycast(origin + i*rayPos.y*Vector3.up, direction, length))
            {
                if(lookingRight)
                    collidingRight = true;
                else
                    collidingLeft = true;

                break;
            }
        }

        direction = CurrentVelocity.y <= 0 ? Vector3.down : Vector3.up;
        origin = this.transform.position + new Vector3(-rayPos.x , direction.y*rayPos.y); 
        length = Mathf.Abs(CurrentVelocity.y) *Time.fixedDeltaTime;

        if(CurrentVelocity.y == 0)
            length += 0.02f;

        for(int i = 0; i < 3; i++)
        {
            //Debug.DrawRay(origin + i*rayPos.x*Vector3.right, direction * length, Color.red);

            if(Physics.Raycast(origin + i*rayPos.x*Vector3.right, direction, length))
            {
                if(CurrentVelocity.y <= 0)
                    onTheGround = true;
                else
                    collidingUp = true;

                break;
            }
        }

        if((collidingLeft || collidingRight) && !onTheGround && !onTheWall)
        {
            onTheWall = true;
            timeToUnstick = wallStickTime;
        }

        if( onTheWall && timeToUnstick >= 0)
        {
            timeToUnstick -= Time.deltaTime;
        }

    }

    void ResetInputs()
    {
        //JumpInput = false;
        //RunInput = false;
    }

    void ResetCollisions()
    {
            collidingLeft = false;
            collidingRight = false;

            if(timeToUnstick < 0)
            {
                onTheWall = false;
                timeToUnstick = 0.0f;
            }

            onTheGround = false;
            collidingUp = false;
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
        if(JumpInput && onTheGround)
        {
            onTheGround = false;
            CurrentVelocity = CurrentVelocity + Vector3.up * JumpSpeed;
        }
    }

    void DoubleJump()
    {

    }

    void WallJump()
    {

        if(JumpInput && onTheWall)
        {
            if(Horizontal != 0)
            {
                CurrentVelocity = CurrentVelocity + Vector3.up * JumpSpeed;
                timeToUnstick = 0.0f;
                onTheWall = false;
            }

            if(Horizontal == 0)
            {
                CurrentVelocity = CurrentVelocity + Vector3.up * JumpSpeed*0.5f;
                timeToUnstick = 0.0f;
                onTheWall = false;
            }

        }
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

