using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceTimeManagement : MonoBehaviour
{

    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
            audioSource.Pause();
        else if (!audioSource.isPlaying)
            audioSource.UnPause();
    }
}
