using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private  PlayerController player;
    public GameObject gameOverPanel;
    private CanvasGroup deathCanvasGroup;
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
        deathCanvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
        if (deathCanvasGroup == null)
        {
            deathCanvasGroup = gameOverPanel.AddComponent<CanvasGroup>();
        }
        deathCanvasGroup.alpha = 0;
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        deathCanvasGroup.alpha = 0.4f;
        Sequence deathSequence = DOTween.Sequence();
        deathSequence.Append(gameOverPanel.transform.DOScale(1.1f, 3f).From(1.3f).SetEase(Ease.InOutQuad));
        deathSequence.Join(deathCanvasGroup.DOFade(1, 3f)).SetUpdate(true); // Aumentar la opacidad
        deathSequence.Play();
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
        winPanel.SetActive(true);
        winPanel.transform.localScale = Vector3.zero;
        winPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
        CanvasGroup canvasGroup = winPanel.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.DOFade(1, 0.5f);
        }
        Time.timeScale = 0f;
        UnlockNextLevel();
        


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
