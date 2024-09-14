using System;
using UnityEngine;
using UnityEngine.UI;

//This script handles moving the character on the Y axis, for jumping and gravity

public class characterJump : MonoBehaviour
{
    [Header("Components")]
    [HideInInspector] public Rigidbody body;
    private characterGround ground; 
    [HideInInspector] public Vector3 velocity;
    [SerializeField] movementLimiter moveLimit;
    public Button jumpButton;

    [Header("Jumping Stats")]
    [SerializeField, Range(2f, 5.5f)][Tooltip("Maximum jump height")] public float jumpHeight = 7.3f;
    [SerializeField, Range(0.2f, 1.25f)][Tooltip("How long it takes to reach the maximum height")] public float timeToJumpApex = 0.5f;
    [SerializeField, Range(0f, 5f)][Tooltip("Gravity multiplier when rising")] public float upwardMovementMultiplier = 1f;
    [SerializeField, Range(1f, 10f)][Tooltip("Gravity multiplier when falling")] public float downwardMovementMultiplier = 6.17f;
    [SerializeField, Range(0, 1)][Tooltip("Air jumps allowed")] public int maxAirJumps = 0;

    [Header("Options")]
    [Tooltip("Variable jump height control")] public bool variableJumpHeight = true;
    [SerializeField, Range(1f, 10f)][Tooltip("Gravity multiplier on jump release")] public float jumpCutOff = 2f;
    [SerializeField, Tooltip("Max falling speed")] public float speedLimit = 20f;
    [SerializeField, Range(0f, 0.3f)][Tooltip("Coyote time duration")] public float coyoteTime = 0.15f;
    [SerializeField, Range(0f, 0.3f)][Tooltip("Jump buffer duration")] public float jumpBuffer = 0.15f;

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
        body = GetComponent<Rigidbody>();
        ground = GetComponent<characterGround>(); 
    }

    private void OnJumpButtonPressed()
    {
        desiredJump = true;
    }

    void Update()
    {
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

    private void PerformJump()
    {
        if (onGround || (coyoteTimeCounter > 0 && coyoteTimeCounter < coyoteTime) || canJumpAgain)
        {
            desiredJump = false;
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;
            canJumpAgain = maxAirJumps > 0 && !canJumpAgain;

            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * body.mass * jumpHeight);

            if (velocity.y > 0f) jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            else if (velocity.y < 0f) jumpSpeed += Mathf.Abs(body.velocity.y);

            velocity.y += jumpSpeed;
            currentlyJumping = true;
        }
    }

    private void CalculateGravity()
    {
        float gravityScale = upwardMovementMultiplier;
        if (body.velocity.y > 0.01f)
        {
            gravityScale = variableJumpHeight && pressingJump ? upwardMovementMultiplier : jumpCutOff;
        }
        else if (body.velocity.y < -0.01f)
        {
            gravityScale = downwardMovementMultiplier;
        }
        body.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, float.MaxValue), velocity.z);
    }

}