using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


public class GameManager : MonoBehaviour
{
    public int playerProgress = 0;
    public SoundSettings soundSettings;
    public AudioClip victorySong;
    public AudioClip defeatSong;
    public ButtonSounds buttonSounds;

    static GameManager instance;
    private void Awake()
    {
        playerProgress = PlayerPrefs.GetInt("PlayerProgress", playerProgress);
        GetSoundSettings();

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

    [System.Serializable]
    public class ButtonSounds
    {
        public AudioClip hoverSound;
        public AudioClip clickSound;
        public AudioClip backSound;
    }

}
