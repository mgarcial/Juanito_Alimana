using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEvent : MonoBehaviour
{

    public void loadLevel1()
    {
        SceneManager.LoadScene("Testing 1"); 
    }

    public void loadLevel2()
    {
        SceneManager.LoadScene("Testing 2");
    }

}
