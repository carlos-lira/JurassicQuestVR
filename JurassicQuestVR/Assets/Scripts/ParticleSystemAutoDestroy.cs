using UnityEngine;
using System.Collections;

public class ParticleSystemAutoDestroy : MonoBehaviour
{
    public float delay = 2.0f;

    public void Update()
    {
        Destroy(gameObject, delay);
    }

}