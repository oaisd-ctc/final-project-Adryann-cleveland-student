using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class gameoverscreen : MonoBehaviour
{
   public void RestartGame()
    {
       
        SceneManager.LoadSceneAsync(1);
    }

    public void MenuScreen()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
