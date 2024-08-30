using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerController : MonoBehaviour
{
    [Header("PlayerStats")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravityScale = 5f;

    public Joystick joystick;
    public Button jumpButton;

    private Rigidbody rb;
    private bool isGrounded;
    public bool playerFacingRight;

    void Start()
    {
        jumpButton.onClick.AddListener(OnJumpButtonPressed);
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void Update()
    {
        Move();
        FlipCharacter();
        ApplyCustomGravity();
    }

    private void Move()
    {
        float moveInput = joystick.Horizontal;
        Vector3 moveDirection = new Vector3(moveInput, 0f, 0f) * moveSpeed;
        rb.velocity = new Vector3(moveDirection.x,rb.velocity.y,rb.velocity.z);
    }

    private void OnJumpButtonPressed()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        if(isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    private void ApplyCustomGravity()
    {
        rb.AddForce(Vector3.down * gravityScale, ForceMode.Acceleration);
    }

    private void FlipCharacter()
    {
        float moveInput = joystick.Horizontal;

        if (moveInput > 0)
        {
            playerFacingRight = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            playerFacingRight = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

}
