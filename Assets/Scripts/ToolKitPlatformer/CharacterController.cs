using UnityEngine;

//This script handles moving the character on the X axis, both on the ground and in the air.

public class characterController : MonoBehaviour, IPickableGun
{

    [Header("Components")]
    [SerializeField] private movementLimiter moveLimit; // Updated for 3D
    private Rigidbody body; // Use Rigidbody for 3D
    private characterGround ground; // Update ground check for 3D
    public Joystick joystick;


    [Header("Movement Stats")]
    [SerializeField, Range(0f, 20f)][Tooltip("Maximum movement speed")] public float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to reach max speed")] public float maxAcceleration = 52f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop after letting go")] public float maxDecceleration = 52f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop when changing direction")] public float maxTurnSpeed = 80f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to reach max speed when in mid-air")] public float maxAirAcceleration;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop in mid-air when no direction is used")] public float maxAirDeceleration;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop when changing direction when in mid-air")] public float maxAirTurnSpeed = 80f;
    [SerializeField][Tooltip("Friction to apply against movement on stick")] private float friction;

    [Header("Options")]
    [Tooltip("When false, the character will skip acceleration and deceleration and instantly move and stop")] public bool useAcceleration;
    public bool itsTheIntro = true;

    [Header("Calculations")]
    public float directionX;
    private Vector3 desiredVelocity; // Change to Vector3 for 3D movement
    public Vector3 velocity; // Change to Vector3
    private float maxSpeedChange;
    private float acceleration;
    private float deceleration;
    private float turnSpeed;

    [Header("Current State")]
    public bool onGround;
    public bool pressingKey;
    private bool playerFacingRight;

    [Header("GunThings")]
    private Gun currentGun;
    public Transform weaponHolder;
    private bool isEquipped = false;


    private void Awake()
    {
        // Find the character's Rigidbody and ground detection script
        body = GetComponent<Rigidbody>(); // Use Rigidbody for 3D physics
        ground = GetComponent<characterGround>(); // Make sure this is adapted for 3D
    }

    private void Update()
    {
        // Get movement input using the old Input System's GetAxis method
        if (moveLimit.CharacterCanMove)
        {
            Debug.Log("I can move");
            directionX = joystick.Horizontal;
        }

        // Used to stop movement when the character is playing her death animation
        if (!moveLimit.CharacterCanMove && !itsTheIntro)
        {
            Debug.Log("I cant move");
            directionX = 0;
        }

        // Flip character model based on movement direction
        if (directionX > 0)
        {
            playerFacingRight = true;
            transform.localScale = new Vector3(1, 1, 1);
            pressingKey = true;
        }
        else if (directionX < 0)
        {
            playerFacingRight = false;
            transform.localScale = new Vector3(1, 1, -1);
            pressingKey = true;
        }
        else
        {
            pressingKey = false;
        }

        // Calculate the desired velocity in 3D (X-axis movement only)
        desiredVelocity = new Vector3(directionX, 0f, 0f) * Mathf.Max(maxSpeed - friction, 0f);
    }

    private void FixedUpdate()
    {
        // Sync with Unity's physics engine
        onGround = ground.GetOnGround(); 
        velocity = body.velocity;

        if (useAcceleration)
        {
            RunWithAcceleration();
        }
        else
        {
            if (onGround)
            {
                RunWithoutAcceleration();
            }
            else
            {
                RunWithAcceleration();
            }
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
    public void OnShootButtonPressed()
    {
        if (currentGun != null)
        {
            currentGun.Shoot();
        }
    }

    private void RunWithAcceleration()
    {
        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        deceleration = onGround ? maxDecceleration : maxAirDeceleration;
        turnSpeed = onGround ? maxTurnSpeed : maxAirTurnSpeed;

        if (pressingKey)
        {
            if (Mathf.Sign(directionX) != Mathf.Sign(velocity.x))
            {
                maxSpeedChange = turnSpeed * Time.deltaTime;
            }
            else
            {
                maxSpeedChange = acceleration * Time.deltaTime;
            }
        }
        else
        {
            maxSpeedChange = deceleration * Time.deltaTime;
        }

        // Apply the velocity change on the X-axis for 3D Rigidbody
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        body.velocity = velocity;
    }

    private void RunWithoutAcceleration()
    {
        velocity.x = desiredVelocity.x;
        body.velocity = velocity;
    }

    public bool IsFacingRight()
    {
        return playerFacingRight;
    }

}