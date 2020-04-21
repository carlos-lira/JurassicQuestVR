using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonFX : MonoBehaviour
{
    AudioSource audioSource;
    AudioClip hoverSound;
    AudioClip clickSound;
    AudioClip backSound;
    GameManager gameManager;
    SoundSettings soundSettings;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        hoverSound = gameManager.buttonSounds.hoverSound;
        clickSound = gameManager.buttonSounds.clickSound;
        backSound = gameManager.buttonSounds.backSound;
    }

    private void Update()
    {
        soundSettings = gameManager.soundSettings;
    }

    public void HoverSound()
    {
        audioSource.clip = hoverSound;
        PlaySound();
    }

    public void ClickSound()
    {
        audioSource.clip = clickSound;
        PlaySound();
    }

    public void BackSound()
    {
        audioSource.clip = backSound;
        PlaySound();
    }

    void PlaySound()
    {
        audioSource.volume = soundSettings.sfxVolume * soundSettings.masterVolumne;
        audioSource.Play();
    }
}
