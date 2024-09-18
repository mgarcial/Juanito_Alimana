using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public CharacterController player;
    public GameManager gameManager;
    private void OnTriggerEnter(Collider other)
    {   
        if(other.CompareTag("Player"))
        {
            Debug.Log($"Ahi ta {player}");
            player = other.GetComponent<CharacterController>();
            player.isDead = true;
            gameManager.RestartGame();
        }
    }
}
