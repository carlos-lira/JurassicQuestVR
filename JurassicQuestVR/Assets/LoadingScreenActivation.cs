using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenActivation : MonoBehaviour
{
    public OVROverlay skybox;
    public OVROverlay canvas;

    private void OnEnable()
    {
        skybox.enabled = true;
        canvas.enabled = true;
    }

    private void OnDisable()
    {
        skybox.enabled = false;
        canvas.enabled = false;
    }
}
