using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class KillPlayer : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {   
        if(other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
            //Debug.Log($"Ahi ta {player}");
            gameManager.GameOver();
        }
    }
}
