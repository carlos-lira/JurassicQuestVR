using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool gameIsPaused = false;

    public GameObject mainPlayer;
    public GameObject menuPlayer;
    public LevelManager levelManager;

    public GameObject pausePanel;
    public GameObject victoryPanel;
    public GameObject defeatPanel;

    private void Awake()
    {
        gameIsPaused = false;
    }

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start) && !levelManager.IsGameOver())
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        pausePanel.SetActive(false);
        //HIDE UI
        EnableObjects();

        //If the first fadein wasnt completed, we fade in again
        if (mainPlayer.GetComponentInChildren<OVRScreenFade>(true).currentAlpha != 0)
            //I created this new function on the Oculus SDK to complete a fade in
            mainPlayer.GetComponentInChildren<OVRScreenFade>(true).FadeInModified();

        //REENABLE BEHAVIOURS
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        pausePanel.SetActive(true);
        //SHOW UI
        DisableObjects();
        //DISABLE UNWANTED BEHAVIOURS
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void DisableObjects()
    {
        menuPlayer.SetActive(true);
        mainPlayer.SetActive(false);
    }

    public void EnableObjects()
    {
        menuPlayer.SetActive(false);
        mainPlayer.SetActive(true);
    }

    public void ShowVictoryScreen()
    {
        victoryPanel.SetActive(true);
        //SHOW UI
        DisableObjects();
        menuPlayer.GetComponentInChildren<OVRScreenFade>(true).FadeIn();
    }

    public void ShowDefeatScreen()
    {
        defeatPanel.SetActive(true);
        //SHOW UI
        DisableObjects();
        menuPlayer.GetComponentInChildren<OVRScreenFade>(true).FadeIn();
    }
}

