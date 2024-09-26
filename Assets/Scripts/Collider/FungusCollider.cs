using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus; 

public class FungusCollider : MonoBehaviour
{
    public Flowchart flowChart;
    public string blockName; 

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            flowChart.ExecuteBlock(blockName);
        }
    }

}
