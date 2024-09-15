using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f;
    public Transform pointA;
    public Transform pointB;
    [SerializeField] private Transform currentTarget;
    public LayerMask groundLayer;

    public bool movingRight = true;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentTarget = pointB;
    }

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        movingRight = direction.x > 0;
        if (Vector3.Distance(transform.position, currentTarget.position) < 2f)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA; 
            Flip();
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
