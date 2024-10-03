using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus; 

public class FungusCollider : MonoBehaviour
{
    public Flowchart flowchart;
    public string blockName;
    public GameObject player;
    private bool hasActivated = false;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !hasActivated) 
        {
            flowchart.ExecuteBlock(blockName); 
            player.GetComponent<CharacterController>().enabled = false;
            player.GetComponent<characterJump>().enabled = false;
            hasActivated = true;
        }
    }

    void Update()
    {
        if(!flowchart.HasExecutingBlocks()) 
        { 
            player.GetComponent<CharacterController>().enabled = true;
            player.GetComponent<characterJump>().enabled = true;
        }
    }
}
