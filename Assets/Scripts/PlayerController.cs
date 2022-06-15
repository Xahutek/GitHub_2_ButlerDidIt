using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : InteractableCharacter
{
    public GameObject Body;

    public bool Invisible
    {
        get { return !Body.activeSelf; }
        set { Body.SetActive(!value); }
    }

    public static PlayerController main;
    static bool isControlled;

    private CapsuleCollider2D col;
    private Rigidbody2D rb;
    public Transform body;
    public FrameInput Input { get; private set; }
    public Vector2 position { get { return transform.position; } set { transform.position = value; lastpos = value; } }
    public Vector2 groundPosition;

    [SerializeField] private Vector2 movement;
    private float currentHorizontalSpeed, currentVerticalSpeed;
    [HideInInspector]public float isMovingHorizontally, isMovingVertically;

    public override void Awake()
    {
        base.Awake();
        if (main == null)
            main = this;
        col= GetComponent<CapsuleCollider2D>();
        rb= GetComponent<Rigidbody2D>();
        lastpos = transform.position;
    }

    private void FixedUpdate()
    {
        LastInputCheck();

    }
    private void Update()
    {
        currentHorizontalSpeed = 0;
        currentVerticalSpeed = 0;
        RunCollisionChecks();

        if (!isControlled && !Portal.isTravelling && !GameManager.isPaused)
        {
            GatherInput();

            CalculateGravity();

            CalculateWalk();
            CalculateJump();
        }
        ApplyMovement();

        ManageVisuals();

    }

    private void GatherInput()
    {
        Input = new FrameInput
        {
            SprintDown = UnityEngine.Input.GetKeyDown(KeyCode.LeftShift),
            SprintUp = UnityEngine.Input.GetKeyUp(KeyCode.LeftShift),
            Sprinting = UnityEngine.Input.GetKey(KeyCode.LeftShift),
            JumpDown = UnityEngine.Input.GetKeyDown(KeyCode.Space),
            JumpUp = UnityEngine.Input.GetKeyUp(KeyCode.Space),
            Jumping = UnityEngine.Input.GetKey(KeyCode.Space),

            X = UnityEngine.Input.GetAxisRaw("Horizontal"),
            Y = UnityEngine.Input.GetAxisRaw("Vertical")
        };
        if (Input.JumpDown)
            lastJumpPressedTime = Time.time;
        if (Input.JumpUp&&isJumping)
            lastJumpReleaseTime = Time.time;
        if (Input.SprintDown)
            lastSprintPressedTime = Time.time;
        if (Input.SprintUp)
            lastSprintReleaseTime = Time.time;
    }

    public Vector2 lastpos;
    private void LastInputCheck()
    {
        float 
            Xdiff = position.x - lastpos.x,
            Ydiff = position.y - lastpos.y;

        isMovingHorizontally = Xdiff;
        isMovingVertically = Ydiff;

        lastpos = position;
    }

    #region Collision

    [SerializeField] private LayerMask LMAll,LMWalls,LMOneWayFloors;
    [HideInInspector]public bool
        grounded;
    private bool
        touchWallLeft,
        touchWallRight;
    private float lastGroundedTime;
    [SerializeField] private float groundCheckWith;

    private void RunCollisionChecks()
    {
        float
            extentsY = col.bounds.extents.y,
            extentsX = col.bounds.extents.x;

        Vector2
            direction1 = Vector2.down * 0.02f,
            direction2 = Vector2.down * 0.02f,
            origin1 = position + Vector2.left * groundCheckWith + Vector2.down * (extentsY - 0.01f),
            origin2 = position + Vector2.right * groundCheckWith + Vector2.down * (extentsY - 0.01f);

        RaycastHit2D
            hitGroundLong = Physics2D.Raycast(position, Vector2.down, extentsY + 1.5f, LMAll),
            hitGround1 = Physics2D.Raycast(origin1, Vector2.down, direction1.magnitude, LMWalls),
            hitGround2 = Physics2D.Raycast(origin2, Vector2.down, direction2.magnitude, LMWalls),
            hitGroundOW1 = Physics2D.Raycast(origin1, Vector2.down, direction1.magnitude, LMOneWayFloors),
            hitGroundOW2 = Physics2D.Raycast(origin2, Vector2.down, direction2.magnitude, LMOneWayFloors),
            hitWallLeft = Physics2D.Raycast(position, Vector2.left, extentsX + 0.01f, LMWalls),
            hitWallRight = Physics2D.Raycast(position, Vector2.right, extentsX + 0.01f, LMWalls);

        groundPosition = hitGroundLong ? hitGroundLong.point : position;

        if (hitGround1 || hitGround2 || hitGroundOW1 || hitGroundOW2)
        {
            grounded = true;
            lastGroundedTime = Time.time;
        }
        else grounded = false;

        touchWallLeft = hitWallLeft;
        touchWallRight = hitWallRight;

        Debug.DrawRay(origin1 + Vector2.up * extentsY, Vector2.down * extentsY, hitGround1 ? Color.green : (hitGroundOW1 ? Color.yellow : Color.red));
        Debug.DrawRay(origin2 + Vector2.up * extentsY, Vector2.down * extentsY, hitGround2 ? Color.green : (hitGroundOW2 ? Color.yellow : Color.red));
        Debug.DrawRay(position, Vector2.left * extentsX, hitWallLeft ? Color.green : Color.red);
        Debug.DrawRay(position, Vector2.right * extentsX, hitWallRight ? Color.green : Color.red);
    }

    #endregion

    #region Walk

    public bool isSprinting;
    private float currentMoveSpeed;

    [SerializeField] private float baseMoveSpeed, baseSprintSpeed;
    [SerializeField] private AnimationCurve 
        SprintAccelerationCurve,
        SprintDecelerationCurve;

    private float 
        lastSprintPressedTime,
        lastSprintReleaseTime;


    public void CalculateWalk()
    {
        isSprinting = Input.Sprinting;
        currentMoveSpeed =
            Mathf.Lerp(baseMoveSpeed,baseSprintSpeed, (isSprinting ?
            SprintAccelerationCurve.Evaluate(Time.time - lastSprintPressedTime) :
            SprintDecelerationCurve.Evaluate(Time.time - lastSprintReleaseTime)));

        currentHorizontalSpeed = currentMoveSpeed * Input.X;
    }

    #endregion

    #region Gravity

    [SerializeField]
    private float gravityForce;
    private float currentFallSpeed;

    public void CalculateGravity()
    {
        if (grounded)
        {
            currentFallSpeed = 0;
        }
        else
        {
            currentFallSpeed += Mathf.Min(Time.deltaTime * gravityForce,20)
                *((isJumping&&currentVerticalSpeed<0)||wasJumping?jumpDownGravityMultiplyer:1);
        }

        currentVerticalSpeed -= currentFallSpeed;
    }

    #endregion

    #region Jump

    [SerializeField]private bool isJumping,wasJumping;
    [SerializeField]
    private float
        jumpPower,
        jumpDownGravityMultiplyer;

    private float 
        lastGroundedY,
        lastJumpPressedTime,
        lastJumpReleaseTime,
        jumpTime;
    [SerializeField] private float jumpQueuedTime;

    public void CalculateJump()
    {
        if(grounded)
            wasJumping = false;

        if (grounded && (Input.JumpDown || lastJumpPressedTime + jumpQueuedTime >= Time.time))
        {
            lastGroundedY = position.y;
            isJumping = true;
        }

        if (isJumping)
        {
            jumpTime += Time.deltaTime;

            currentVerticalSpeed += jumpPower;

            if (Input.JumpUp || (grounded&&jumpTime>0.1))
            {
                isJumping = false;
                wasJumping=true;
                if (currentVerticalSpeed > 0) currentFallSpeed = 0;
                else currentFallSpeed -= jumpPower;
                lastJumpReleaseTime = Time.time;
                jumpTime = 0;
            }
        }
    }

    #endregion

    public void ApplyMovement()
    {
        movement = new Vector2(currentHorizontalSpeed, currentVerticalSpeed);

        rb.velocity = movement;
    }

    public void ManageVisuals()
    {
        if(currentHorizontalSpeed!=0)
        body.localScale = new Vector3(Mathf.Abs(body.localScale.x)*Mathf.Sign(currentHorizontalSpeed), body.localScale.y, body.localScale.z);
    }
    public void RefreshCollider()
    {
        col.enabled=false;
        col.enabled = true;
    }
}

public struct FrameInput
{
    public float X,Y;
    public bool 
        SprintDown,
        SprintUp,
        Sprinting;
    public bool 
        JumpDown,
        JumpUp,
        Jumping;
}
