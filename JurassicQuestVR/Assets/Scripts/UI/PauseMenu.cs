using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool gameIsPaused = false;

    public LevelManager levelManager;
    public GameObject mainPlayer;
    public GameObject pauseMenu;

    public GameObject pausePanel;
    public GameObject victoryPanel;
    public GameObject defeatPanel;

    public AudioSource[] audioSources;

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
        UnpauseAudioSources();
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
        PauseAudioSources();
        //SHOW UI
        DisableObjects();
        //DISABLE UNWANTED BEHAVIOURS
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void DisableObjects()
    {
        pauseMenu.SetActive(true);
        mainPlayer.SetActive(false);
    }

    public void EnableObjects()
    {
        pauseMenu.SetActive(false);
        mainPlayer.SetActive(true);
    }

    public void ShowVictoryScreen()
    {
        victoryPanel.SetActive(true);
        //SHOW UI
        DisableObjects();
        pauseMenu.GetComponentInChildren<OVRScreenFade>(true).FadeIn();
    }

    public void ShowDefeatScreen()
    {
        defeatPanel.SetActive(true);
        //SHOW UI
        DisableObjects();
        pauseMenu.GetComponentInChildren<OVRScreenFade>(true).FadeIn();
    }

    public void PauseAudioSources()
    {
        if (audioSources != null)
        {
            foreach (var source in audioSources)
                source.Pause();
        }
    }

    public void UnpauseAudioSources()
    {
        if (audioSources != null)
        {
            foreach (var source in audioSources)
                source.UnPause();
        }
    }

    public bool GameIsPaused()
    {
        return gameIsPaused;
    }
}

