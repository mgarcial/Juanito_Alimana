using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform weaponHolder;  // El transform del Weapon Holder
    public Transform character;     // El transform del personaje
    public float groundCheckDistance = 0.1f;  // Distancia para verificar si est� en el suelo
    public LayerMask groundLayer;    // El layer que representa el suelo
    private bool isJumping = false;
    private Vector3 holderDefaultRotation;  // Rotaci�n original del Weapon Holder

    void Start()
    {
        // Guardar la rotaci�n original del Weapon Holder
        holderDefaultRotation = weaponHolder.localEulerAngles;
    }

    void Update()
    {
        CheckIfJumping();
    }

    void CheckIfJumping()
    {
        // Verificar si el personaje est� en el suelo usando un Raycast
        bool isGrounded = Physics2D.Raycast(character.position, Vector2.down, groundCheckDistance, groundLayer);

        if (!isGrounded && !isJumping)
        {
            // Si no est� en el suelo y a�n no est� en modo de salto
            isJumping = true;
            RotateHolderDown();
        }
        else if (isGrounded && isJumping)
        {
            // Si ha aterrizado y estaba saltando
            isJumping = false;
            ResetHolderRotation();
        }
    }

    void RotateHolderDown()
    {
        // Cambiar la rotaci�n del Weapon Holder para que apunte hacia abajo
        weaponHolder.localEulerAngles = new Vector3(0, 90, -90);  // Ajusta el valor seg�n el eje y rotaci�n
    }

    void ResetHolderRotation()
    {
        // Restaurar la rotaci�n original del Weapon Holder
        weaponHolder.localEulerAngles = holderDefaultRotation;
    }
}
