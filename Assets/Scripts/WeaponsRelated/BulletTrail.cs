using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    public float speed = 50f;
    public Vector3 targetPosition;
    public float lifetime = 2f;
    private float fixedZPosition;

    private void Start()
    {
        fixedZPosition = transform.position.z;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        Vector3 newPosition = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        transform.position = new Vector3(newPosition.x, newPosition.y, fixedZPosition);
        if (transform.position == targetPosition)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
