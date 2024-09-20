using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class characterController : MonoBehaviour, IPickableGun
{
    [Header("PlayerStats")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravityScale = 5f;

    public Joystick joystick;
    public Button jumpButton;

    private Rigidbody rb;
    private Gun currentGun;
    public Transform weaponHolder;
    private bool isGrounded;
    private bool isEquipped = false;
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
    public void OnShootButtonPressed()
    {
        if (currentGun != null)
        {
            currentGun.Shoot();
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
            transform.forward = new Vector3(-1, 0, 0);
        }
        else if (moveInput < 0)
        {
            playerFacingRight = false;
            transform.forward = new Vector3(1, 0, 0); 
        }
    }

    public void PickUpGun(Gun gun)
    {
        if (isEquipped)
        {
            Destroy(currentGun.gameObject);
            Debug.Log($"Destroying {currentGun}");
        }

        currentGun = Instantiate(gun, weaponHolder.position, weaponHolder.rotation, weaponHolder);

        currentGun.firePoint = currentGun.transform.Find("FirePoint");
        currentGun.SetGunHolder(this);
        isEquipped = true;
        Debug.Log("Picked up gun: " + currentGun);
    }

    public bool IsFacingRight()
    {
        return playerFacingRight;
    }
}
