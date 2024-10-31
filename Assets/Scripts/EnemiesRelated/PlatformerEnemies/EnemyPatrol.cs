using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyPatrol : MonoBehaviour
{
    private enum State
    {
        Patrolling,
        ChasePlayer,
        BackToStart
    }
    [Header("EnemyyStats")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 1f;

    public float distanceToPoint = 2f;
    public float detectionRange = 10f;
    public float stopChaseDistance = 15f;
    [Header("Enemy Checks")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Transform groundFallCheckPoint;
    [SerializeField] private float checkDistance = 1.0f;
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private bool aboutToFall;
    [SerializeField] private bool noGround;
    [SerializeField] private Transform currentTarget;
    [SerializeField] private State state;
    [SerializeField] private bool playerDetected = false;
    [SerializeField] Collider2D enemyCollider;
    public bool movingRight = true;
    public Vector3 playerPosition;
    [Header("Things to Assign")]
    [SerializeField] private Transform startingTarget;
    public Transform pointA;
    public Transform pointB;

    private Vector3 startingPosition;
    private Rigidbody2D rb;

    private void Awake()
    {
        state = State.Patrolling;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = startingTarget;
        startingPosition = transform.position;
    }

    private void Update()
    {
        switch(state)
        {
            default:
            case State.Patrolling:
                Patrol();
                CheckIsAboutToFall();
                CheckGround();
                FindPlayer();
                break;
            case State.ChasePlayer:
                CheckIsAboutToFall();
                CheckGround();
                ChasePlayer();
                break;
            case State.BackToStart:
                BackToStart();
                CheckGround();
                break;
        }

    }

    private void BackToStart()
    {
        currentTarget = startingTarget;
        Vector3 direction = (startingPosition - transform.position).normalized;
        rb.MovePosition(transform.position + direction * patrolSpeed * Time.deltaTime);
        movingRight = direction.x > 0;
        if (direction.x > 0 && transform.localScale.z < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Abs(transform.localScale.z));
        }
        else if (direction.x < 0 && transform.localScale.z > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -Mathf.Abs(transform.localScale.z));
        }
        if (Vector3.Distance(transform.position, startingPosition) < 2f)
        {
            state = State.Patrolling;
        }
    }

    private void Patrol()
    {
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * patrolSpeed * Time.deltaTime);
        movingRight = direction.x > 0;
        if (Vector3.Distance(transform.position, currentTarget.position) < distanceToPoint)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA; 
            Flip();
        }
    }

    private void ChasePlayer()
    {
        currentTarget = PlayerController.Instance.GetTransform();
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * chaseSpeed * Time.deltaTime);
        movingRight = direction.x > 0;
        if (direction.x > 0 && transform.localScale.z > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -Mathf.Abs(transform.localScale.z));
        }
        else if (direction.x < 0 && transform.localScale.z < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Abs(transform.localScale.z));
        }
        if (Vector2.Distance(transform.position, currentTarget.position) > detectionRange)
        {
            playerDetected = false;
            state = State.BackToStart;
        }
    }
    private void FindPlayer()
    {
        playerPosition = PlayerController.Instance.GetPosition();
        if (Vector3.Distance(transform.position, playerPosition) < detectionRange)
        {
            playerDetected = true;
            state = State.ChasePlayer;
        }  
        else
        {
            playerDetected = false;
        }
    }

    private void CheckIsAboutToFall()
    {
        Debug.DrawRay(groundCheckPoint.position, Vector3.down, Color.yellow, 3.0f);
        if (Physics2D.Raycast(groundCheckPoint.position, Vector3.down, checkDistance, groundLayer))
        {
            aboutToFall = false;
        }
        else
        {
            aboutToFall = true;
            state = State.BackToStart;
            //rb.gravityScale = 1000;
        }
    }
    private void CheckGround()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector3.down, checkDistance, groundLayer))
        {
            noGround = false;          
        }
        else
        {
            noGround= true;
            enemyCollider.isTrigger = true;
        }
    }
    public bool IsAboutToFall()
    {
        return aboutToFall;
    }

    private void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -transform.localScale.z);
    }
    public bool IsMovingRight()
    {
        return movingRight;
    }
}
