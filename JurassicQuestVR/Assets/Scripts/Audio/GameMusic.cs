using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    public AudioClip levelSong;

    GameObject gameManager;
    AudioClip victorySong;
    AudioClip defeatSong;

    SoundSettings soundSettings;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        soundSettings = gameManager.GetComponent<GameManager>().soundSettings;
        victorySong = gameManager.GetComponent<GameManager>().victorySong;
        defeatSong = gameManager.GetComponent<GameManager>().defeatSong;

        audioSource = GetComponent<AudioSource>();
        UpdateAudioSettings();
        audioSource.clip = levelSong;
        audioSource.loop = true;

        audioSource.Play();
    }

    void Update()
    {
        UpdateAudioSettings();
    }

    void UpdateAudioSettings()
    {
        audioSource.mute = soundSettings.muted;
        audioSource.volume = soundSettings.musicVolume * soundSettings.masterVolumne * 0.5f;
    }

    public void PlayVictorySong()
    {
        audioSource.Stop();
        audioSource.clip = victorySong;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayDefeatSong()
    {
        audioSource.Stop();
        audioSource.clip = defeatSong;
        audioSource.loop = false;
        audioSource.Play();
    }

}
