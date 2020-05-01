using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateAudio : MonoBehaviour
{
    GameManager gameManager;
    public Slider slider;
    public Toggle checkBox;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (slider == null) 
            slider = GetComponentInChildren<Slider>();
        slider.value = gameManager.soundSettings.musicVolume;

        if (checkBox == null)
            checkBox = GetComponentInChildren<Toggle>();
        checkBox.isOn = gameManager.soundSettings.muted;
    }

    public void MuteMusic()
    {
        gameManager.soundSettings.muted = checkBox.isOn;
    }

    public void UpdateMusicLevel()
    {
        gameManager.soundSettings.musicVolume = slider.value;
    }

    private void OnDisable()
    {
        if (gameObject.activeSelf)
            SaveMusicSettings();
    }

    public void SaveMusicSettings()
    {
        Debug.Log("Sound Saved");
        int muteStatus = gameManager.soundSettings.muted ? 1 : 0;
        PlayerPrefs.SetFloat("MusicVolume", gameManager.soundSettings.musicVolume);
        PlayerPrefs.SetInt("MusicMuted", muteStatus);
    }


}
