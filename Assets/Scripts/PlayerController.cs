using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float DASH_DURATION = 0.15f;

    Rigidbody rb;
    [SerializeField]
    float JumpSpeed = 8f;
    bool canDash = true;
    
    public Vector3 CurrentVelocity { get; protected set; }

    public float Horizontal { get; protected set; }
    public bool JumpInput { get; protected set; }
    public bool RunInput { get; protected set; }
    public bool GrabInput { get; protected set; }
    public bool DashInput { get; protected set; }

    public float Speed = 10f;
    public float runSpeed = 10f;
    public float climbingSpeed = 3f;
    private float wallStickTime = 0.5f;
    private float timeToUnstick = 0.0f;
    [SerializeField]
    private float dashSpeed = 50f;
    [SerializeField]
    private float currentDashDuration = 0f;
    [SerializeField]
    private float dashCooldown = 3f;
    [SerializeField]
    private float nextDash;
    public float currentDashTime;
    [SerializeField]
    private float direction;
    [SerializeField]
    private bool isDashing;
    [SerializeField]
    private bool canDoubleJump;
    [SerializeField]
    private bool canJump;
    public LayerMask groundLayer;

    public float MovableBoxDetectorDistance = 0.75f;

    public LayerMask MovableBoxMask;

    GameObject MovableBox;
    
    #region RUN_DASH
    
    #endregion

    #region COLLISION_INFO
    private bool lookingRight = true;
    [SerializeField]
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

        Jump();
        DoubleJump();
        WallJump();
        Walk();
        Run();
        Dash();

        ApplyToRigidbody();

        ResetInputs();
        ResetCollisions();

    }

    void GetInputs()
    {
        Horizontal = Input.GetAxis("Horizontal");
        JumpInput = Input.GetKeyDown(KeyCode.Space);
        RunInput = Input.GetKey(KeyCode.LeftShift);
        DashInput = Input.GetKeyDown(KeyCode.LeftControl);
        
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
        if(Horizontal != 0) {
            lookingRight = Horizontal > 0;
        }

        Vector3 direction = lookingRight ? Vector3.right : Vector3.left;
        Vector3 origin = this.transform.position + new Vector3(direction.x*rayPos.x , -rayPos.y); 
        float length =  Horizontal * Speed * Time.fixedDeltaTime;

        if(Horizontal == 0)
            length += 0.02f;
        
        for(int i = 0; i < 3; i++)
        {
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
            if(Physics.Raycast(origin + i*rayPos.x*Vector3.right, direction, length))
            {
                if(CurrentVelocity.y <= 0)
                {
                    onTheGround = true;
                } 
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
        if(JumpInput && canJump)
        {    
            CurrentVelocity = CurrentVelocity + Vector3.up * JumpSpeed;
            canJump = false;
            canDoubleJump = true;
        }
    }

    void DoubleJump()
    {
        if(CurrentVelocity.y < 0 && canDoubleJump && JumpInput)
        {
            CurrentVelocity = CurrentVelocity + Vector3.up * JumpSpeed;
            canDoubleJump = false;
        }
    }

    void WallJump()
    {

        if(JumpInput && onTheWall)
        {
            Vector3 direction = new Vector3(0, 0, 0);

            if (collidingLeft) {
                direction = new Vector3(1 * Speed, CurrentVelocity.y, 0);
            } else {
                direction = new Vector3(-1 * Speed, CurrentVelocity.y, 0);
            }

            CurrentVelocity = CurrentVelocity + Vector3.up * (JumpSpeed * 0.2f) + direction;
            timeToUnstick = 0.0f;
            onTheWall = false;
        }
    }

    void Walk()
    {
        if (!onTheWall) {
            if (Horizontal > 0) gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (Horizontal < 0) gameObject.transform.rotation = Quaternion.Euler(0, 0, 180f);

            CurrentVelocity = new Vector3(Horizontal * Speed, CurrentVelocity.y, 0);
        }
    }

    void Run()
    {
        if(RunInput)
        {
            CurrentVelocity = CurrentVelocity + (Vector3.right * Horizontal * runSpeed);
        }
    }

    void Dash() {
        if (DashInput && canDash) {
            currentDashDuration = DASH_DURATION;
            canDash = false;
            isDashing = true;
            nextDash = Time.time + dashCooldown;
            direction = this.lookingRight ? 1f : -1f;
            StartCoroutine(EDash());
        }


        if (currentDashDuration > 0) {
            CurrentVelocity = Vector3.right * direction * dashSpeed;
            currentDashDuration -= Time.deltaTime;
        }
    }
    
    IEnumerator EDash() {
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "Movable Box"){
            MovableBox = collisionInfo.gameObject;
        }
        if(collisionInfo.gameObject.tag == "Killer Plane")
        {
            LevelStartPoint startPoint = GameObject.FindObjectOfType<LevelStartPoint>();
            transform.position = startPoint.transform.position;
        }
        Debug.Log(collisionInfo.gameObject.tag);
        if(collisionInfo.gameObject.layer == 6)
        {
            canJump = true;
            canDoubleJump = false;
        }
    }
}
