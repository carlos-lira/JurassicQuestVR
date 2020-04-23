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
    GameMusic gameMusic;


    bool hasWon = false;
    bool hasLost = false;

    public virtual void Start()
    {
        gameMusic = GetComponent<GameMusic>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
        gameManager.RestartMission();
        //SceneManager.LoadSceneAsync(GetCurrentScene(), LoadSceneMode.Additive);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



    IEnumerator Win()
    {
        Debug.Log("YOU WON!");
        //Save progress
        if (gameManager != null)
            gameManager.SaveProgress();

        //FadeOut
        Time.timeScale = 0.5f;
        gameMusic.PlayVictorySong();

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
        gameMusic.PlayDefeatSong();

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

    private int GetCurrentScene()
    {
        int currentScene = 0;
        int countLoaded = SceneManager.sceneCount;
        Scene[] loadedScenes = new Scene[countLoaded];

        for (int i = 0; i < countLoaded; i++)
        {
            if (SceneManager.GetSceneAt(i).buildIndex > currentScene)
                currentScene = SceneManager.GetSceneAt(i).buildIndex;
        }

        return currentScene;
    }

}

