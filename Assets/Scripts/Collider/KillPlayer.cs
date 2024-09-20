using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class KillPlayer : MonoBehaviour
{
    public CharacterController player;
    public GameManager gameManager;

    public GameObject gameOverPanel;

    public void Start()
    {
        gameOverPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {   
        if(other.CompareTag("Player"))
        {
            gameOverPanel.SetActive(true);
            /*Debug.Log($"Ahi ta {player}");
            player = other.GetComponent<CharacterController>();
            player.isDead = true;
            gameManager.RestartGame(); */
            Time.timeScale = 0; 
            
        }
    }
    public void Retry()
    {
        SceneManager.LoadScene("Level 1");
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f; 
    }

}
