using UnityEngine;

//This script handles moving the character on the X axis, both on the ground and in the air.

public class PlayerController : MonoBehaviour, IPickableGun, IDamageable
{
    public static PlayerController Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private MovementLimiter moveLimit; 
    private Rigidbody2D body;
    private CharacterGround ground; 
    public Joystick joystick;
    public GameObject hitEffects;


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
    public bool isDead = false;

    [Header("GunThings")]
    private Gun currentGun;
    public Transform weaponHolder;
    private bool isGunEquipped = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (Instance != this)
        {
            Destroy(gameObject);
        }
        // Find the character's Rigidbody and ground detection script
        body = GetComponent<Rigidbody2D>(); // Use Rigidbody for 3D physics
        ground = GetComponent<CharacterGround>(); // Make sure this is adapted for 3D
    }

    private void Update()
    {
        // Get movement input using the old Input System's GetAxis method
        if (moveLimit.CharacterCanMove)
        {

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
        if (!isGunEquipped)
        {
            currentGun = Instantiate(gun, weaponHolder.position, weaponHolder.rotation, weaponHolder);
            currentGun.SetGunHolder(this);
            isGunEquipped = true;
            Debug.Log("Picked up gun: " + currentGun);           
        }
        if (isGunEquipped && currentGun != null)
        {
            Gun previousGun = currentGun;
            isGunEquipped=false;
            currentGun = Instantiate(gun, weaponHolder.position, weaponHolder.rotation, weaponHolder);
            currentGun.SetGunHolder(this);
            isGunEquipped = true;
            Debug.Log("Changed up last gun for: " + currentGun);
            Debug.Log($"now the last gun is{previousGun}");
            Destroy(previousGun.gameObject);
        }
        
    }
    
    public bool IsWeaponEquipped()
    {
        return isGunEquipped;
    }
    public void OnShootButtonPressed()
    {
        if (currentGun != null && currentGun.canShoot)
        {
            currentGun.bulletsShot = currentGun.bulletPerTap;
            currentGun.Shoot();
            currentGun.canShoot = false;
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

    public void TakeHit()
    {
        if(hitEffects != null)
        {
            AudioManager.GetInstance().PlayHitPlayerSound();
            hitEffects.SetActive(true);
            Invoke("DeactivateParticles", 1f);
        }
    }

    public void Die()
    {
       if(isDead)
        {
            AudioManager.GetInstance().PlayDeathSound();
            //Destroy(gameObject);
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    public Transform GetTransform()
    {
        return transform;
    }
    private void DeactivateParticles()
    {
        hitEffects.SetActive(false);
    }
}