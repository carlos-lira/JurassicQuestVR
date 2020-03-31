using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOnCollision : MonoBehaviour
{
    public OVRScreenFade fader = null;
    public float fadeTime = 0.01f;

	private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 || other.gameObject.layer == 11 || other.gameObject.layer == 15)
            fader.FadeOutUpdated(fadeTime);

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10 || other.gameObject.layer == 11 || other.gameObject.layer == 15)
            fader.FadeInUpdated(fadeTime);
    }

}


