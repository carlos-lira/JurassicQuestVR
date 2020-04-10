using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool gameIsPaused = false;

    public GameObject mainPlayer;
    public GameObject menuPlayer;

    private void Awake()
    {
        gameIsPaused = false;
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
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
        //HIDE UI
        EnableObjects();

        //REENABLE BEHAVIOURS
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        //SHOW UI
        DisableObjects();

        //DISABLE UNWANTED BEHAVIOURS
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    void DisableObjects()
    {
        menuPlayer.SetActive(true);
        mainPlayer.SetActive(false);
    }

    void EnableObjects()
    {
        menuPlayer.SetActive(false);
        mainPlayer.SetActive(true);
    }
}

