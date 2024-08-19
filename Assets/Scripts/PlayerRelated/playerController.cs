using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    CharacterController characterController;
    public Joystick joystick;
    [Header("Character stats")]
    [SerializeField] private float speed;
    private float verticalVel;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime = 0.2f;
    [SerializeField] private float jumpTimer;
    [SerializeField] private float mass = 8.0f;
    [Header("Forces")]
    [SerializeField] private float Gravity = 9.81f;
    [SerializeField] private float friction = 0.5f;
    [Header("Checks & Validations")]
    [SerializeField] private bool IsGrounded;
    [SerializeField] private float groundedTimer;
    public bool playerFacingRight = true;
    private bool jumpRequested;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontalInput = Mathf.Clamp(joystick.Horizontal, -1f, 1f);
        Vector3 move = new Vector3 (horizontalInput, 0, 0) * speed;
        move *= speed;
        IsGrounded = characterController.isGrounded;

        if (IsGrounded)
        {
            move.x *= 1 - friction * Time.deltaTime;
            move.z *= 1 - friction * Time.deltaTime;
            groundedTimer = 0.2f;
            verticalVel = 0;
        }
       
        groundedTimer -= groundedTimer > 0 ? Time.deltaTime : 0;

        verticalVel -= (Gravity/mass) * Time.deltaTime;

        jumpRequested = Input.GetButton("Jump");

        if (jumpRequested && groundedTimer > 0)
        {
            groundedTimer = 0;
            jumpTimer = Mathf.Max(jumpTimer-Time.deltaTime, 0);
            verticalVel += Mathf.Sqrt(jumpForce * 2 * Gravity / mass) * (jumpTimer > 0 ? 1f : 0.5f);
        }

        jumpTimer = jumpRequested ? Mathf.Max(jumpTimer - Time.deltaTime, 0) : 0;
        
        move.y = verticalVel;
        characterController.Move(move*Time.deltaTime);

        if (horizontalInput > 0 && !playerFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && playerFacingRight)
        {
            Flip();
        }
    }

    public void SetMass(float newMass)
    {
        mass = newMass; 
    }
    private void Flip()
    {
        playerFacingRight = !playerFacingRight;
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }

}
