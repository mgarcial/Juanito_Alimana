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
            player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<CharacterJump>().enabled = false;
            hasActivated = true;
        }
    }

    void Update()
    {
        if(hasActivated && !flowchart.HasExecutingBlocks()) 
        { 
            player.GetComponent<PlayerController>().enabled = true;
            player.GetComponent<CharacterJump>().enabled = true;
        }
    }
}
