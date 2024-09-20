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
    public float speed = 2f;
    public float distanceToPoint = 2f;
    public float detectionRange = 10f;
    public float stopChaseDistance = 15f;
    [Header("Enemy Checks")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float checkDistance = 1.0f;
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private bool aboutToFall;
    [SerializeField] private Transform currentTarget;
    [SerializeField] private State state;
    [SerializeField] private bool playerDetected = false;
    public bool movingRight = true;
    public Vector3 playerPosition;
    [Header("Things to Assign")]
    [SerializeField] private Transform startingTarget;
    public Transform pointA;
    public Transform pointB;

    private Vector3 startingPosition;
    private Rigidbody rb;

    private void Awake()
    {
        state = State.Patrolling;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
                CheckGround();
                FindPlayer();
                break;
            case State.ChasePlayer:
                CheckGround();
                ChasePlayer();
                break;
            case State.BackToStart:
                BackToStart();
                break;
        }

    }

    private void BackToStart()
    {
        currentTarget = startingTarget;
        Vector3 direction = (startingPosition - transform.position).normalized;
        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
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
        currentTarget = CharacterController.Instance.GetTransform();
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        movingRight = direction.x > 0;
        if (direction.x > 0 && transform.localScale.z < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Abs(transform.localScale.z));
        }
        else if (direction.x < 0 && transform.localScale.z > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -Mathf.Abs(transform.localScale.z));
        }
        if (Vector3.Distance(transform.position, playerPosition) > detectionRange)
        {
            playerDetected = false;
            state = State.BackToStart;
        }
    }
    private void FindPlayer()
    {
        playerPosition = CharacterController.Instance.GetPosition();
        if (Vector3.Distance(transform.position, playerPosition) < detectionRange)
        {
            playerDetected = true;
            state = State.ChasePlayer;
        }    
    }

    private void CheckGround()
    {
        RaycastHit hit;
        Debug.DrawRay(groundCheckPoint.position, Vector3.down, Color.yellow, 3.0f);
        if (Physics.Raycast(groundCheckPoint.position, Vector3.down, out hit, checkDistance, groundLayer))
        {
            aboutToFall = false;
        }
        else
        {
            aboutToFall = true;
            state = State.BackToStart;
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
