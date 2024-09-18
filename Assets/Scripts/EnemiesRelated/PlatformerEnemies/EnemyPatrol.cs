using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyPatrol : MonoBehaviour
{
    private enum State
    {
        Patrolling,
        ChasePlayer
    }
    public float speed = 2f;
    public Transform pointA;
    public Transform pointB;
    public float distanceToPoint = 2f;
    public float detectionRange = 10f;
    [SerializeField] private Transform currentTarget;
    public Vector3 playerPosition;
    private State state;
    private bool playerDetected = false;
    public bool movingRight = true;
    private Rigidbody rb;

    private void Awake()
    {
        state = State.Patrolling;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentTarget = pointB;
    }

    private void Update()
    {
        switch(state)
        {
            default:
            case State.Patrolling:
                Patrol();
                FindPlayer();
                break;
            case State.ChasePlayer:
                ChasePlayer();
                break;
        }

    }

    private void Patrol()
    {
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        movingRight = direction.x > 0;
        if (Vector3.Distance(transform.position, currentTarget.position) < distanceToPoint)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA; 
            Flip();
        }
    }

    private void ChasePlayer()
    {
        currentTarget.position = playerPosition;
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        movingRight = direction.x > 0;
        if (Vector3.Distance(transform.position, currentTarget.position) < detectionRange)
        {
            playerDetected = true;  
        }
        if (direction.x > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    private void FindPlayer()
    {
        playerPosition = CharacterController.Instance.GetPosition();
        if (Vector3.Distance(transform.position, playerPosition) < detectionRange)
        {
            state = State.ChasePlayer;
        }
    }
    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    public bool IsMovingRight()
    {
        return movingRight;
    }
}
