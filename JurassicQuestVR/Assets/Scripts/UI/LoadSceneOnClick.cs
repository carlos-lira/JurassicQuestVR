using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadSceneOnClick : MonoBehaviour
{
    public GameManagerLoader gml;
    public void LoadByIndex(int sceneIndex) 
    {
        EnableTimer();
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadByEnum(SceneIndexes sceneIndex)
    {
        EnableTimer();
        SceneManager.LoadScene((int)sceneIndex);
    }

    public void LoadMission()
    {
        EnableTimer();
        gml.LoadMission(GetComponentInParent<DisplayQuest>().questId);
    }

    public void LoadSelectedScene()
    {
        EnableTimer();
        SceneManager.LoadScene(GetComponentInParent<DisplayQuest>().questId);
    }

    public void RestartScene()
    {
        EnableTimer();
        GameObject.Find("LevelManager").GetComponent<LevelManager>().Restart();
    }

    private void EnableTimer() 
    {
        Time.timeScale = 1f;
    }

}
