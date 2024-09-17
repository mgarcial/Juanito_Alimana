using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class IndoPanelDetectable : MonoBehaviour
{
    public GameObject infoPanel; 
    

    void Start()
    {
        infoPanel.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            infoPanel.SetActive(true); 
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            infoPanel.SetActive(false); 
        }
    }
}
