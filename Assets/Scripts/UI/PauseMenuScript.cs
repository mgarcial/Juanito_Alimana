using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using DG.Tweening;
using System.Threading.Tasks;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pausePanel;
    private RectTransform pauseRectTransform;

    private void Start()
    {
        pauseRectTransform = pausePanel.GetComponent<RectTransform>();
        pauseRectTransform.anchoredPosition = new Vector2(0, Screen.height);

    }
    void Update()
    {
        
    }
    
    public void Pause()
    {
        AudioManager.GetInstance().PlaySoundButton();
        pausePanel.SetActive(true);
        pauseRectTransform.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutExpo).SetUpdate(true);
        Time.timeScale = 0;
        Fungus.Flowchart.BroadcastFungusMessage("PauseDialogues");
    }

    public void Resume()
    {
        AudioManager.GetInstance().PlayConfirmButton();
        pauseRectTransform.DOAnchorPos(new Vector2(0, Screen.height), 1f).SetEase(Ease.InExpo)
            .OnComplete(() => pausePanel.SetActive(false));
        Time.timeScale = 1;
        Fungus.Flowchart.BroadcastFungusMessage("ResumeDialogues"); 
    }

    public void Home()
    {
        AudioManager.GetInstance().PlaySoundButton();
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }
}
