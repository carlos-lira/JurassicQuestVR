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

    public AudioClip victorySong;
    public AudioClip defeatSong;
    public ButtonSounds buttonSounds;

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    static GameManager instance;
    private void Awake()
    {
        /*
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        */
        instance = this;
        playerProgress = PlayerPrefs.GetInt("PlayerProgress", playerProgress);
        GetSoundSettings();

        if (!debugMode)
        {
            SceneManager.LoadSceneAsync((int)SceneIndexes.MAIN_MENU, LoadSceneMode.Additive);
            //FirstGameLoad();
        }
    }

    private void Start()
    {
    }

    public void FirstGameLoad()
    {
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.MAIN_MENU, LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress());
    }

    public void LoadMission(int missionId)
    {
        loadingScreen.SetActive(true);

        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.MAIN_MENU));
        scenesLoading.Add(SceneManager.LoadSceneAsync(missionId, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public void GoToMainMenu()
    {
        int currentScene = GetCurrentScene();

        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync(currentScene));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.MAIN_MENU, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public void GameRestart()
    {
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.MANAGER, LoadSceneMode.Single));

        StartCoroutine(GetSceneLoadProgress());
    }


    public void RestartMission()
    {
        int currentScene = GetCurrentScene();

        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync(currentScene));
        scenesLoading.Add(SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public IEnumerator GetSceneLoadProgress()
    {
        foreach (var scene in scenesLoading)
        {
            while (!scene.isDone)
            { 
                yield return null;
            }
        }

        scenesLoading.Clear();
        loadingScreen.SetActive(false);
    }

    void GetSoundSettings()
    {
        soundSettings = new SoundSettings();
        soundSettings.musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);

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
