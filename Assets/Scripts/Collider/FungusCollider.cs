using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusCollider : MonoBehaviour
{
    public Flowchart flowChart;
    public string blockName;
    private bool hasActivated = false; 

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !hasActivated)
        {
            flowChart.ExecuteBlock(blockName);
            hasActivated = true;
        }
    }

    private void Update()
    {
        if(!flowChart.HasExecutingBlocks())
        {

        }
    }
}
