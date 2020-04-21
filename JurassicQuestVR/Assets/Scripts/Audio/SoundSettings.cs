using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundSettings
{
    public bool muted = false;
    [HideInInspector]
    [Range(0f,1f)]
    public float masterVolumne = 1f;
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    [Range(0f, 1f)]
    public float sfxVolume = 0.7f;
    [Range(0f, 1f)]
    public float voicesVolume = 1f;

}
