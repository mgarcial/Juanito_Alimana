using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    CharacterController characterController;
    [SerializeField] private float speed;
    [SerializeField] private float verticalVel;
    [SerializeField] private float jumpForce;
    [SerializeField] private float Gravity = 9.81f;
    [SerializeField] private bool IsGrounded;
    [SerializeField] private float groundedTimer;
    [SerializeField] private float mass = 8.0f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        IsGrounded = characterController.isGrounded;
        if (IsGrounded)
        {
            groundedTimer = 0.2f;
        }
        if(groundedTimer > 0f)
        {
            groundedTimer -= Time.deltaTime;
        }

        if (IsGrounded && verticalVel < 0f)

        {
            verticalVel = 0f;
        }

        verticalVel -= (Gravity/mass) * Time.deltaTime;

        Vector3 move = new Vector3 (Input.GetAxis("Horizontal"), 0, 0);
        move *= speed;

        if(Input.GetButtonDown("Jump"))
        {
            if(groundedTimer > 0)
            {
                groundedTimer = 0;
                verticalVel += Mathf.Sqrt(jumpForce * 2 * Gravity/mass);
            }
        }

        move.y = verticalVel;
        characterController.Move(move*Time.deltaTime);
    }

    public void SetMass(float newMass)
    {
        mass = newMass; 
    }

}
