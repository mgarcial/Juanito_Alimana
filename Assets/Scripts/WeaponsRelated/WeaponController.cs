using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform weaponHolder;  
    public Transform character;    
    public float groundCheckDistance = 0.1f; 
    public LayerMask groundLayer;    
    private bool isJumping = false;
    private Vector3 holderDefaultRotation; 

    void Start()
    {
        holderDefaultRotation = weaponHolder.localEulerAngles;
    }

    void Update()
    {
        CheckIfJumping();
    }

    void CheckIfJumping()
    {

        bool isGrounded = Physics2D.Raycast(character.position, Vector2.down, groundCheckDistance, groundLayer);

        if (!isGrounded && !isJumping)
        {
            isJumping = true;
            RotateHolderDown();
        }
        else if (isGrounded && isJumping)
        {
            isJumping = false;
            ResetHolderRotation();
        }
    }

    void RotateHolderDown()
    {
        weaponHolder.localEulerAngles = new Vector3(0, 90, -90);  
    }

    void ResetHolderRotation()
    {
        weaponHolder.localEulerAngles = holderDefaultRotation;
    }
}
