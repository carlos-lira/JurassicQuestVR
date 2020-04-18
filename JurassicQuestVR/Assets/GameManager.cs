using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


public class GameManager : MonoBehaviour
{
    public int playerProgress = 0;

    static GameManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        playerProgress = PlayerPrefs.GetInt("PlayerProgress", playerProgress);
    }

    public void SaveProgress(int completedLevelId)
    {
        if (completedLevelId > playerProgress)
        {
            playerProgress = completedLevelId;
            PlayerPrefs.SetInt("PlayerProgress", playerProgress);
        }
    }

    public void DeleteProgress()
    {
        PlayerPrefs.DeleteKey("PlayerProgress");
        playerProgress = 0;
    }
}
