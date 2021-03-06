﻿using System.Collections;
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
        //Music at volume 1 is too disruptive for gameplay. This will reduce it to 50% of the selected value.
        //Changed back to 1 with the new music
        audioSource.volume = soundSettings.musicVolume * soundSettings.masterVolumne * 1f;
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
