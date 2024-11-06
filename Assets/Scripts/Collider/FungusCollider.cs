using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus; 

public class FungusCollider : MonoBehaviour
{
    public Flowchart flowchart;
    public string blockName;
    public GameObject player;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool hasActivated;

    public void OnTriggerEnter2D(Collider2D other)
    {
        rb = other.GetComponentInParent<Rigidbody2D>();
        if(other.CompareTag("Player") && !hasActivated) 
        {
            hasActivated = true;
            PlayerController playerController = other.GetComponent<PlayerController>();
            flowchart.ExecuteBlock(blockName);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    void Update()
    {
        if(hasActivated && !flowchart.HasExecutingBlocks()) 
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
