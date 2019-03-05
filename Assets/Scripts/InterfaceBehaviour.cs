using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Ui;

public class InterfaceBehaviour : MonoBehaviour
{
    public GameLogic gameLogic;

    public Panel mainMenuScreen;
    public Panel splashScreen;
    public Panel loadingScreen;
    public Panel optionsScreen;
    public Panel traderScreen;
    public Panel endGameScreen;
    public Panel sagaScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadGame()
    {
        mainMenuScreen.SetActive(false);
        loadingScreen.SetActive(true);
    }
    void EnableOptionsMenu()
    {
        optionsScreen.SetActive(true);
        mainMenuScreen.SetActive(false);
    }

    void DisableoptionsMenu()
    {
        mainMenuScreen.SetActive(true);
        optionsScreen.SetActive(false);
    }
    void EnableOptionsMenuInGame()
    {

    }

    IEnumerator RunLoadingScreenanimation()
    {
        // do the animations animations #endregion
        // start the game via the game logic 
    }
}
