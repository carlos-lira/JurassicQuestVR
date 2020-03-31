using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public void EndGame(bool victory)
    {
        if (victory)
        {
            Debug.LogWarning("YOU WON!");
        }
        else 
        {
            Debug.Log("YOU LOST!");
        }
    }

    public void Restart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
