using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pausePanel; 

    void Update()
    {
        
    }
    
    public void Pause()
    {
        AudioManager.GetInstance().PlaySoundButton();
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        Fungus.Flowchart.BroadcastFungusMessage("PauseDialogues");
    }

    public void Resume()
    {
        AudioManager.GetInstance().PlayConfirmButton();
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        Fungus.Flowchart.BroadcastFungusMessage("ResumeDialogues"); 
    }

    public void Home()
    {
        AudioManager.GetInstance().PlaySoundButton();
        SceneManager.LoadScene("Menu"); 
    }
}
