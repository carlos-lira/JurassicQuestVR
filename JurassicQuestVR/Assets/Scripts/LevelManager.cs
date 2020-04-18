using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public bool combatEnabled = true;

    [System.NonSerialized]
    public GameManager gameManager;

    public int deadEnemies;

    OVRScreenFade fader;
    PauseMenu pauseMenu;

    bool hasWon = false;
    bool hasLost = false;

    public virtual void Start()
    {
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        pauseMenu = GetComponent<PauseMenu>();
        fader = pauseMenu.mainPlayer.GetComponentInChildren<OVRScreenFade>(true);
        hasWon = false;
        hasLost = false;
        deadEnemies = 0;
    }


    public void EnemyKilled()
    {
        deadEnemies++;
    }

    public void EndGame(bool victory)
    {
        if (victory && !IsGameOver())
        {
            hasWon = true;
            StartCoroutine(Win());
            //Save progress
           if (GameObject.Find("GameManager").GetComponent<GameManager>() != null) GameObject.Find("GameManager").GetComponent<GameManager>().SaveProgress(SceneManager.GetActiveScene().buildIndex);
        }
        else if (!victory && !IsGameOver())
        {
            hasLost = true;
            StartCoroutine(Lose());
        }
    }

    public void Restart()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Win()
    {
        Debug.Log("YOU WON!");

        //FadeOut
        Time.timeScale = 0.5f;

        fader.FadeOut();
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f;
        pauseMenu.ShowVictoryScreen();

        yield return null;
    }

    IEnumerator Lose()
    {
        Debug.Log("YOU LOST!");

        //FadeOut
        Time.timeScale = 0.5f;

        fader.FadeOut();
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f;
        pauseMenu.ShowDefeatScreen();

        yield return null;
    }

    public bool IsGameOver()
    {
        return (hasWon || hasLost);
    }


}

