using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerLoader : MonoBehaviour
{
    public GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void LoadMission(int missionId)
    {
        gameManager.LoadMission(missionId);
    }

    public void GoToMainMenu()
    {
        gameManager.GoToMainMenu();
    }

    public void RestartMission()
    {
        gameManager.RestartMission();
    }
    
    public void GameRestart()
    {
        gameManager.GameRestart();
    }

}
