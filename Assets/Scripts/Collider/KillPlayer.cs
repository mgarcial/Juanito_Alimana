using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class KillPlayer : MonoBehaviour
{
    public CharacterController player;
    public GameManager gameManager;
    private void OnTriggerEnter2D(Collider2D other)
    {   
        if(other.CompareTag("Player"))
        {
            Debug.Log($"Ahi ta {player}");
            player = other.GetComponent<CharacterController>();
            gameManager.GameOver();
        }
    }
}
