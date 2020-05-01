using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasPanelDisplay : MonoBehaviour
{
    GameManager gameManager;
    public GameObject mainMenu;
    public GameObject levelSelector;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (gameManager != null)
        {
            if (gameManager.GetPreviousScene() == (int)SceneIndexes.MANAGER || gameManager.GetPreviousScene() == (int)SceneIndexes.MAIN_MENU)
            {
                mainMenu.SetActive(true);
                levelSelector.SetActive(false);
            }
            else 
            {
                mainMenu.SetActive(false);
                levelSelector.SetActive(true);
            }
        }
    }

}
