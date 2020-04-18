using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaCorpLogic : LevelManager
{
    bool reachedPortal;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        reachedPortal = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (reachedPortal)
            EndGame(true);
    }

    public void ReachedPortal()
    {
        reachedPortal = true;
    }
}
