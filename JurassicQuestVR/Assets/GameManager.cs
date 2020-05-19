using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


public class GameManager : MonoBehaviour
{
    public bool debugMode = false;

    public int playerProgress = 0;
    public SoundSettings soundSettings;
    public GameObject loadingScreen;
    public GameObject pauseMenu;

    public AudioClip victorySong;
    public AudioClip defeatSong;
    public ButtonSounds buttonSounds;

    int previousScene = 0;
    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    static GameManager instance;
    private void Awake()
    {
        instance = this;
        playerProgress = PlayerPrefs.GetInt("PlayerProgress", playerProgress);
        GetSoundSettings();

        if (!debugMode)
        {
            FirstGameLoad();
        }
    }

    private void Start()
    {
    }

    public void FirstGameLoad()
    {
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.MAIN_MENU, LoadSceneMode.Additive));
        //StartCoroutine(FirstSceneLoadProgress((int)SceneIndexes.MAIN_MENU));
        StartCoroutine(GetSceneLoadProgress((int)SceneIndexes.MAIN_MENU));
    }

    public void LoadMission(int missionId)
    {
        loadingScreen.SetActive(true);

        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.MAIN_MENU));
        scenesLoading.Add(SceneManager.LoadSceneAsync(missionId, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress(missionId));
    }

    public void GoToMainMenu()
    {
        int currentScene = GetCurrentScene();

        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync(currentScene));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.MAIN_MENU, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress((int)SceneIndexes.MAIN_MENU));
    }

    public void GameRestart()
    {
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.MANAGER, LoadSceneMode.Single));

        StartCoroutine(GetSceneLoadProgress((int)SceneIndexes.MANAGER));
    }


    public void RestartMission()
    {
        int currentScene = GetCurrentScene();

        loadingScreen.SetActive(true);
        
        scenesLoading.Add(SceneManager.UnloadSceneAsync(currentScene));
        scenesLoading.Add(SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress(currentScene));
        
        //StartCoroutine(ReloadScene());
    }

    IEnumerator ReloadScene()
    {
        int currentScene = GetCurrentScene();
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));
        scenesLoading.Add(SceneManager.UnloadSceneAsync(currentScene));
        foreach (var scene in scenesLoading)
        {
            while (!scene.isDone)
            {
                yield return null;
            }
        }

        scenesLoading.Clear();
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.MAIN_MENU, LoadSceneMode.Additive));
        foreach (var scene in scenesLoading)
        {
            while (!scene.isDone)
            {
                yield return null;
            }
        }

        scenesLoading.Clear();
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.MAIN_MENU));
        foreach (var scene in scenesLoading)
        {
            while (!scene.isDone)
            {
                yield return null;
            }
        }

        Debug.LogWarning("AQUIIIIIIIIIIIIIIIIIIIIIIIIII");
        Debug.LogWarning(currentScene);

        scenesLoading.Clear();
        scenesLoading.Add(SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive));
        foreach (var scene in scenesLoading)
        {
            while (!scene.isDone)
            {
                yield return null;
            }
        }

        scenesLoading.Clear();
        Time.timeScale = 1f;
        loadingScreen.SetActive(false);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(currentScene));
    }


    public IEnumerator GetSceneLoadProgress(int newScene)
    {

        previousScene = GetCurrentScene();

        float loadingDuration = 0f;
        foreach (var scene in scenesLoading)
        {
            while (!scene.isDone)
            {
                loadingDuration += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        scenesLoading.Clear();
        Time.timeScale = 1f;
        loadingScreen.SetActive(false);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(newScene));
    }

    public IEnumerator FirstSceneLoadProgress(int newScene)
    {

        previousScene = GetCurrentScene();

        float loadingDuration = 0f;
        foreach (var scene in scenesLoading)
        {
            while (!scene.isDone)
            {
                loadingDuration += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        scenesLoading.Clear();
        Time.timeScale = 1f;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(newScene));

        //New
        scenesLoading.Add(SceneManager.UnloadSceneAsync(newScene));
        scenesLoading.Add(SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress(newScene));
    }

    void GetSoundSettings()
    {
        soundSettings = new SoundSettings();
        soundSettings.musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);

        int musicMuted = PlayerPrefs.GetInt("MusicMuted", 0);
        if (musicMuted == 0)
        {
            soundSettings.muted = false;
        }
        else
        {
            soundSettings.muted = true;
        }
    }

    public void SaveProgress()
    {
        int completedLevelId = GetCurrentScene() - 1;
        if (completedLevelId > playerProgress)
        {
            playerProgress = completedLevelId;
            PlayerPrefs.SetInt("PlayerProgress", playerProgress);
        }
    }

    public int GetPreviousScene()
    {
        return previousScene;
    }

    public int GetCurrentScene()
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

    public void SwapPlayers()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = true;
        loadingScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    public void DeleteProgress()
    {
        PlayerPrefs.DeleteKey("PlayerProgress");
        playerProgress = 0;
    }

    [System.Serializable]
    public class ButtonSounds
    {
        public AudioClip hoverSound;
        public AudioClip clickSound;
        public AudioClip backSound;
    }

}
