using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamAlphaLogic : LevelManager
{
    GameObject lauraObject;
    Laura laura;

    int currentPhase = 1;
    public Phase1 phase1;
    public Phase2 phase2;
    public Phase3 phase3;
    public Phase4 phase4;

    public override void Start()
    {
        base.Start();
        lauraObject = GameObject.FindGameObjectWithTag("Laura");
        laura = lauraObject.GetComponent<Laura>();
    }

    private void Update()
    {
        CheckFaseFinished();

        switch (currentPhase)
        {
            case 1:
                Phase1Logic();
                break;
            case 2:
                Phase2Logic();
                break;
            case 3:
                Phase3Logic();
                break;
            case 4:
                Phase4Logic();
                break;
            default:
                break;
        }
    }

    void Phase1Logic()
    {
        //Check if the enemies are dead and player is near laura
        phase1.IsCompleted(deadEnemies, player, lauraObject);
    }

    void Phase2Logic()
    {
        //Check if Laura is standing and play audio
        if (laura.IsStanding() && !phase2.audioPlayed)
        {
            StartCoroutine(laura.TurnToFace(player.transform.position));
            phase2.audioPlayed = true;
            laura.Talk(phase2.audioClip);
        }
        //Check if Laura stopped talking
        else if (!laura.IsTalking() && phase2.audioPlayed && !(phase2.GetDistance(lauraObject.transform.position, phase2.targetPosition.position) <= 1.5f))
        {
            StopAllCoroutines();
            laura.Run(phase2.targetPosition.position);
        }
        //When laura arrives to the destination, check for phase completion
        else if (!laura.IsRunning())
        {
            phase2.IsCompleted(deadEnemies, player, lauraObject);
        }

    }

    void Phase3Logic()
    {
        //Check if Laura is standing and play audio
        if (!phase3.audioPlayed)
        {
            StartCoroutine(laura.TurnToFace(player.transform.position));
            phase3.audioPlayed = true;
            laura.Talk(phase3.audioClip);
        }
        else if (!laura.IsTalking() && phase3.audioPlayed)
        {
            phase3.IsCompleted(deadEnemies, player, lauraObject);
        }
    }

    void Phase4Logic()
    {
        //Check if Laura is standing and play audio
        if (!laura.IsRunning() && !(phase4.GetDistance(lauraObject.transform.position, phase4.targetPosition.position) <= 1.5f))
        {
            StopAllCoroutines();
            laura.Run(phase4.targetPosition.position);
        }
        else if (!laura.IsRunning() && !phase4.audioPlayed)
        {
            phase4.audioPlayed = true;
            laura.Talk(phase4.audioClip);
        }
        else if (laura.IsIdle() && phase4.audioPlayed)
        {
            StartCoroutine(laura.TurnToFace(phase4.raptorPosition.position));
            phase4.IsCompleted(deadEnemies, player, lauraObject);
        }
    }

    void CheckFaseFinished()
    {
        switch (currentPhase)
        {
            case 1:
                if (phase1.completed)
                {
                    if(phase1.boundary != null)
                        phase1.boundary.SetActive(false);
                    currentPhase = 2;
                    InitiatePhase2();
                }
                break;
            case 2:
                if (phase2.completed)
                {
                    if (phase2.boundary != null)
                        phase2.boundary.SetActive(false);
                    currentPhase = 3;
                    InitiatePhase3();
                }
                break;
            case 3:
                if (phase3.completed)
                {
                    if (phase3.boundary != null)
                        phase3.boundary.SetActive(false);
                    currentPhase = 4;
                    InitiatePhase4();
                }
                break;
            case 4:
                if (phase4.completed)
                {
                    EndGame(true);
                }
                break;
            default:
                EndGame(false);
                break;
        }
    }
    void InitiatePhase2() 
    {
        deadEnemies = 0;
        laura.StandUp();
    }
    void InitiatePhase3() 
    {
        deadEnemies = 0;
    }
    void InitiatePhase4() 
    {
        deadEnemies = 0;
    }



    public class Phase
    {
        [System.NonSerialized]
        public bool completed = false;
        [System.NonSerialized]
        public bool audioPlayed = false;

        public int enemiesToKill = 0;

        public float GetDistance(Vector3 v1, Vector3 v2)
        {
            //Ignoring Y axis
            Vector3 distance = v1 - v2;
            distance.y = 0;

            return Mathf.Abs(distance.magnitude);
        }
    }

    [System.Serializable]
    public class Phase1 : Phase
    {
        public float playerDistanceToLaura = 10f;
        public GameObject boundary;

        //3 enemies dead and player near laura
        public bool IsCompleted(int currentKills, GameObject player, GameObject laura)
        {
            if (currentKills == enemiesToKill && GetDistance(laura.transform.position, player.transform.position) <= playerDistanceToLaura)
                completed = true;
            return completed;
        }
    }

    [System.Serializable]
    public class Phase2 : Phase
    {
        public AudioClip audioClip;
        public Transform targetPosition;
        public float playerDistanceToLaura = 10f;
        public GameObject boundary;

        //laura at target position and player near laura
        public bool IsCompleted(int currentKills, GameObject player, GameObject laura)
        {
            if (GetDistance(targetPosition.position, laura.transform.position) <= 1.5f && GetDistance(laura.transform.position, player.transform.position) <= playerDistanceToLaura)
                completed = true;
            return completed;
        }


    }

    [System.Serializable]
    public class Phase3 : Phase
    {
        public AudioClip audioClip;
        public GameObject boundary;

        //2 enemies dead
        public bool IsCompleted(int currentKills, GameObject player, GameObject laura)
        {
            if (currentKills == enemiesToKill)
                completed = true;
            return completed;
        }

    }

    [System.Serializable]
    public class Phase4 : Phase
    {
        public Transform targetPosition;
        public AudioClip audioClip;
        public Transform raptorPosition;

        //laura at end position and audio played
        public bool IsCompleted(int currentKills, GameObject player, GameObject laura)
        {
            if (GetDistance(targetPosition.position, laura.transform.position) <= 1.5 && audioPlayed)
                completed = true;
            return completed;
        }
    }
}
