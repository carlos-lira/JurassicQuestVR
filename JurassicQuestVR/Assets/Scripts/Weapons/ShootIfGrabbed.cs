﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootIfGrabbed : MonoBehaviour
{

    private SimpleShoot simpleShoot;
    private OVRGrabbable ovrGrabbable;
    public OVRInput.Button shootingButton;
    public AudioClip gunShotSound;

    //public Vector3 offset;
    public Transform offset;

    private List<GameObject> enemiesInRange;

    // Start is called before the first frame update
    void Start()
    {
        enemiesInRange = new List<GameObject>();
        simpleShoot = GetComponent<SimpleShoot>();
        ovrGrabbable = GetComponent<OVRGrabbable>();
    }

    // Update is called once per frame
    void Update()
    {

        if (ovrGrabbable.isGrabbed)
        {
            transform.SetParent(ovrGrabbable.grabbedBy.transform);

            transform.position = ovrGrabbable.grabbedBy.transform.position; //+ offset;
            if(ovrGrabbable.grabbedBy.name.Contains("Right"))
                transform.rotation = ovrGrabbable.grabbedBy.transform.rotation * Quaternion.Euler(0, 0, 90);
            else
                transform.rotation = ovrGrabbable.grabbedBy.transform.rotation * Quaternion.Euler(0, 0, -90);

            if (OVRInput.GetDown(shootingButton, ovrGrabbable.grabbedBy.GetController()))
            {
                WarnEnemies();
                VibrationManager.singleton.TriggerVibration(40, 2, 255, ovrGrabbable.grabbedBy.GetController());
                GetComponent<AudioSource>().PlayOneShot(gunShotSound);
                simpleShoot.TriggerShoot();
            }
        }
        else
        {
            transform.SetParent(null);
        }
        
        /*
        if (ovrGrabbable.isGrabbed && OVRInput.GetDown(shootingButton, ovrGrabbable.grabbedBy.GetController())) 
        {
            VibrationManager.singleton.TriggerVibration(40,2,255, ovrGrabbable.grabbedBy.GetController());
            GetComponent<AudioSource>().PlayOneShot(gunShotSound);
            simpleShoot.TriggerShoot();
        }
        */
    }

    private void LateUpdate()
    {
        enemiesInRange.Clear();
    }

    private void WarnEnemies()
    {
        foreach (var enemy in enemiesInRange)
        {
                if (enemy != null)
                    if (enemy.GetComponent<Soldier>() != null)
                        enemy.GetComponent<Soldier>().EnterCombat();


        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 17)
            enemiesInRange.Add(other.gameObject);
    }


}