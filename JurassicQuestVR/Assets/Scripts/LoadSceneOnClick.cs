using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadSceneOnClick : MonoBehaviour
{
    public void LoadByIndex(int sceneIndex) 
    {
        EnableTimer();
        SceneManager.LoadScene(sceneIndex);
    }

    public void RestartScene()
    {
        EnableTimer();
        GameObject.Find("GameManager").GetComponent<GameManager>().Restart();
    }

    private void EnableTimer() 
    {
        Time.timeScale = 1f;
    }
}
