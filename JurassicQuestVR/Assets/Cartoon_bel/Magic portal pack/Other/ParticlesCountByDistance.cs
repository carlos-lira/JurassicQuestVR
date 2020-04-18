using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class ParticlesCountByDistance : MonoBehaviour {

    [Range(0, 100)]
    public float PercentParticlesAfter20Meters = 15;

    int startMaxParticles;
    ParticleSystem.MainModule main;
	// Use this for initialization
	void Start () {
        main = GetComponent<ParticleSystem>().main;
        startMaxParticles = main.maxParticles;
	}
	
	// Update is called once per frame
	void Update () {
        var distance = (Camera.main.transform.position - transform.position).magnitude - 10;
        var multiplier = Mathf.Lerp(1, PercentParticlesAfter20Meters / 100f, distance / 20f);
        main.maxParticles = (int)(startMaxParticles * multiplier);
    }
}