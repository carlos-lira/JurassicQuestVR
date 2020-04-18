using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVictoryGameOverLogic : LevelManager
{
    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (deadEnemies == 1)
            EndGame(true);
    }
}
