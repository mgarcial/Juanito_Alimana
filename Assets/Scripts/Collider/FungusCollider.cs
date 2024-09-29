using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusCollider : MonoBehaviour
{
    public Flowchart flowChart;
    public string blockName;
    public GameObject player;
    private bool hasActivated = false;

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !hasActivated)
        {
            flowChart.ExecuteBlock(blockName);
            player.GetComponent<CharacterController>().enabled = false;
            player.GetComponent<characterJump>().enabled = false;

            hasActivated = true;
        }
    }

    private void Update()
    {
        if(!flowChart.HasExecutingBlocks())
        {
            player.GetComponent<CharacterController>().enabled = true;
            player.GetComponent<characterJump>().enabled = true;
        }
    }
}
