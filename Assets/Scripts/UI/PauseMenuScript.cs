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
    //[SerializeField] RectTransform pausePanelRect; 
    //[SerializeField] float topPosY, midPosY; 
    //[SerializeField] float tweenDuration; 
    //[SerializeField] CanvasGroup canvasGroup; //dark panel 

    void Update()
    {
        
    }
    
    public void Pause()
    {
        AudioManager.GetInstance().PlaySoundButton();
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        // PauseMenuIntro(); 
        Fungus.Flowchart.BroadcastFungusMessage("PauseDialogues");
    }

    public void Resume()
    {
        // PauseMenuOutro(); 
        AudioManager.GetInstance().PlayConfirmButton();
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        Fungus.Flowchart.BroadcastFungusMessage("ResumeDialogues"); 
    }

    public void Home()
    {
        AudioManager.GetInstance().PlaySoundButton();
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

/*
    void PauseMenuIntro()
    {
        canvasGroup.DOFade(1, tweenDuration).SetUpdate(true); 
        pausePanelRect.DOAnchorPosY(midPosY, tweenDuration).SetUpdate(true); 

    }

    void PauseMenuOutro()
    {
        canvasGroup.DOFade(0, tweenDuration).SetUpdate(true); 
         pausePanelRect.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true); 
    } */
}
