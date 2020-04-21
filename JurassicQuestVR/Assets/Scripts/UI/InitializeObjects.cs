using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeObjects : MonoBehaviour
{
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeactivate;

    private void Start()
    {
        foreach (var ob in objectsToDeactivate)
        {
            ob.SetActive(false);
        }
        foreach (var ob in objectsToActivate)
        {
            ob.SetActive(true);
        }
    }
}
