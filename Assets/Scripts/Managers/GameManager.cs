using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private  PlayerController player;
    public GameObject gameOverPanel;
    public GameObject winPanel;
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
        player = FindAnyObjectByType<PlayerController>();
        AudioManager.GetInstance().PlayBackgroundMusic();
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        AudioManager.GetInstance().PlayDeathSound();
        AudioManager.GetInstance().PlayDeathMusic();
        Time.timeScale = 0f;
    }

    public void Retry()
    {
        AudioManager.GetInstance().PlaySoundButton();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameOverPanel.SetActive(false);
        CleanLevel();
    }

    public void CleanLevel()
    {
        //EnemyManager.instance.ClearEnemiesList();
        Time.timeScale = 1f;
    }

    public void NextLevel()
    {
        CleanLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        AudioManager.GetInstance().PlayConfirmButton();
        Destroy(gameObject);
    }
    public void WinLevel()
    {
        Time.timeScale = 0f;
        UnlockNextLevel();
        winPanel.SetActive(true);

    }
    public void Home()
    {
        AudioManager.GetInstance().PlaySoundButton();
        SceneManager.LoadScene("Menu");
    }
    private void UnlockNextLevel()
    {
        int currentLvl = Preferences.GetCurrentLvl();
        Preferences.SetMaxLvl(currentLvl + 1);
    }
    /*
    public void RestartGame()
    {
        if(player.isDead)
        {
            StartCoroutine(ReloadScene());
        }
    }

    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    */
}
