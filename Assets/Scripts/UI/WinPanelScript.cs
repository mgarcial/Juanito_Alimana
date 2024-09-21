using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class WinPanelScript : MonoBehaviour
{

    public void NextLevel()
    {
        SceneManager.LoadScene(""); 

    }

    public void backToHome()
    {
        SceneManager.LoadScene("Menu"); 
    }

    public void Retry()
    {
        SceneManager.LoadScene("Level 1"); 
    }
}
