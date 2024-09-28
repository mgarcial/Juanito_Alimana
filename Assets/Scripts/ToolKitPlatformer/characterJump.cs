using System;
using UnityEngine;
using UnityEngine.UI;

//This script handles moving the character on the Y axis, for jumping and gravity

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(Collider))]
public class characterJump : MonoBehaviour
{
    [Header("Components")]
    [HideInInspector] public Rigidbody2D body;
    private characterGround ground; 
    [HideInInspector] public Vector2 velocity;
    [SerializeField] movementLimiter moveLimit;
    public Button jumpButton;

    [Header("Jumping Stats")]
    [SerializeField, Range(2f, 25f)][Tooltip("Maximum jump height")] public float jumpHeight = 7.3f;
    [SerializeField, Range(0.1f, 5.25f)][Tooltip("How long it takes to reach the maximum height")] public float timeToJumpApex;
    [SerializeField, Range(0f, 5f)][Tooltip("Gravity multiplier when rising")] public float upwardMovementMultiplier = 1f;
    [SerializeField][Tooltip("Gravity multiplier when falling")] public float downwardMovementMultiplier = 6.17f;
    [SerializeField, Range(0, 4)][Tooltip("Air jumps allowed")] public int maxAirJumps = 0;


    [Header("Options")]
    [Tooltip("Variable jump height control")] public bool variableJumpHeight;
    [SerializeField, Range(1f, 50f)][Tooltip("Gravity multiplier on jump release")] public float jumpCutOff = 2f;
    [SerializeField, Tooltip("Max falling speed")] public float speedLimit = 20f;
    [SerializeField, Range(0f, 0.3f)][Tooltip("Coyote time duration")] public float coyoteTime = 0.15f;
    [SerializeField, Range(0f, 0.3f)][Tooltip("Jump buffer duration")] public float jumpBuffer = 0.15f;

    [Header("Calculations")]
    public float jumpSpeed;
    private float defaultGravityScale;
    public float gravMultiplier;

    [Header("Current State")]
    private bool canJumpAgain = false;
    private bool desiredJump;
    private float jumpBufferCounter;
    private float coyoteTimeCounter = 0;
    private bool pressingJump;
    private bool onGround;
    private bool currentlyJumping;

    void Awake()
    {
        jumpButton.onClick.AddListener(OnJumpButtonPressed);
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<characterGround>();
        defaultGravityScale = 1f;
    }

    private void OnJumpButtonPressed()
    {
        desiredJump = true;
    }

    void Update()
    {
        SetPhysics();
        onGround = ground.GetOnGround(); 
        HandleJumpBuffer();
        HandleCoyoteTime();
    }

    void FixedUpdate()
    {
        velocity = body.velocity;
        if (desiredJump)
        {
            Debug.Log("I jumped");
            PerformJump();
            body.velocity = velocity;
            return;
        }
        CalculateGravity();
    }

    private void HandleJumpBuffer()
    {
        if (jumpBuffer > 0 && desiredJump)
        {
            jumpBufferCounter += Time.deltaTime;
            if (jumpBufferCounter > jumpBuffer)
            {
                desiredJump = false;
                jumpBufferCounter = 0;
            }
        }
    }

    private void HandleCoyoteTime()
    {
        if (!currentlyJumping && !onGround)
        {
            coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            coyoteTimeCounter = 0;
        }
    }
    private void SetPhysics()
    {
        Vector2 newGravity = new Vector2(0, (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex));
        body.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravMultiplier;
    }

    private void PerformJump()
    {
        if (onGround || (coyoteTimeCounter > 0.03f && coyoteTimeCounter < coyoteTime) || canJumpAgain)
        {
            desiredJump = false;
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;
            canJumpAgain = maxAirJumps > 0 && !canJumpAgain;
            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * body.gravityScale * jumpHeight);
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(body.velocity.y);
            }

            velocity.y += jumpSpeed;
            currentlyJumping = true;
        }
        if (jumpBuffer == 0)
        {
            //If we don't have a jump buffer, then turn off desiredJump immediately after hitting jumping
            desiredJump = false;
        }
    }

    private void CalculateGravity()
    {

        if (body.velocity.y > 0.01f)
        {
            if (onGround)
            {
                //Don't change it if Kit is stood on something (such as a moving platform)
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                //If we're using variable jump height...)
                if (variableJumpHeight)
                {
                    //Apply upward multiplier if player is rising and holding jump
                    if (pressingJump && currentlyJumping)
                    {
                        gravMultiplier = upwardMovementMultiplier;
                    }
                    //But apply a special downward multiplier if the player lets go of jump
                    else
                    {
                        gravMultiplier = jumpCutOff;
                    }
                }
                else
                {
                    gravMultiplier = upwardMovementMultiplier;
                }
            }
        }
        else if (body.velocity.y < -0.01f)
        {
            if (onGround)
            //Don't change it if Kit is stood on something (such as a moving platform)
            {
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                //Otherwise, apply the downward gravity multiplier as Kit comes back to Earth
                gravMultiplier = downwardMovementMultiplier;
            }
        }
        else
        {
            if (onGround)
            {
                currentlyJumping = false;
            }

            gravMultiplier = defaultGravityScale;
        }
        body.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, 100));
    }

}