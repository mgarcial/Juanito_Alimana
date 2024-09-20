using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private  CharacterController player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        player = FindAnyObjectByType<CharacterController>();
        AudioManager.GetInstance().PlayBackgroundMusic();

    }

    /*
    public void RestartGame()
    {
        if(player.isDead)
        {
            SceneManager.LoadScene("level 1"); 
        }
    }

    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    */
}
