using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEvent : MonoBehaviour
{

    public void loadLevel1()
    {
        SceneManager.LoadScene("Level 1"); 
    }

    public void loadLevel2()
    {
        SceneManager.LoadScene("");
    }

}
