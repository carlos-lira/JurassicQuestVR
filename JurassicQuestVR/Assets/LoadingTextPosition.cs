using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingTextPosition : MonoBehaviour
{
    // Start is called before the first frame update
    public OVROverlay loadingText;
    public Camera persistantCamera;

    void Start()
    {
        WaitforOVROverlay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WaitforOVROverlay()
    {
        Transform camTransform = persistantCamera.transform;
        Transform uiTextOverlayTrasnform = loadingText.transform;
        Vector3 newPos = camTransform.position + camTransform.forward * 3;
        newPos.y = camTransform.position.y;
        uiTextOverlayTrasnform.position = newPos;
        loadingText.enabled = true;

    }
}
