using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceBehaviour : MonoBehaviour
{
    public GameLogic gameLogic;

    public GameObject mainMenuScreen;
    public GameObject splashScreen;
    public GameObject loadingScreen;
    public GameObject optionsScreen;
    public Text optionsBackButtontext;
    public GameObject inGameScreen;
    public GameObject inGameMenu;
    public GameObject traderScreen;
    public GameObject endGameScreen;
    public GameObject sagaScreen;

    public GameObject playerInventory;
    public bool playerInventoryOpened = false;
     public Text inventoryCrewText;
     public Text inventoryFoodText;
     public Text inventoryWeaponText;
     public Text inventoryLootText;
    
    public GameObject actionScreen;
    List<string> hintsAndTrivia = new List<string>()
    {
        "Are we there yet?",
        "Move with W,A,S,D or the arrow keys",
        "Movement requires sustenance!",
        "Never run out of food! Especially mead!",
        "You can open and close the ships inventory with 'I'",
        "You will need weapons to brave the perils Midgard",
        "Vikings devided the year in summer and winter months",
        "Sögur are the tales of worthy men, their voyages and deeds",
        "There is rumours of a secret tower",
        " According to legend, only the bravest vikings who die sword in hand, will enter Valhalla"
    };
    private int m_triviaIndex = 0;
    public Text hintsAndTriviaText;
    void Start()
    {
        // Splash screen
        ShowNextHint();
    }

    void Update()
    {
        if(gameLogic.gameStarted)
        {
            if(Input.GetKeyDown("escape"))
            {
                if(!gameLogic.menuEnabled)
                {
                    EnableGameMenuInGame();
                }
                else
                {
                    DisableGameMenuInGame();
                }
            }

            if(Input.GetKeyDown("i"))
            {
                // toggle player inventory
                playerInventoryOpened = !playerInventoryOpened;
                StartCoroutine("OpenCloseInventory");
            }

            if(Input.GetKeyDown("space"))
            {
                if(gameLogic.playerObject.GetComponent<PlayerBehaviour>().lastLookedAtObject != null )
                {
                    GameTileBehaviour obj = gameLogic.playerObject.GetComponent<PlayerBehaviour>().lastLookedAtObject.GetComponent<GameTileBehaviour>();

                    if( obj.tileType == GameTileTypes.TraderTile || obj.tileType == GameTileTypes.VillageTile || obj.tileType == GameTileTypes.StartVillageTile)
                    {
                        EnableActionScreen();
                        actionScreen.GetComponent<ActionScreenBehaviour>().AdjustActionDialogue(obj.tileType, obj.name);  
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        UpdateInterfaceTexts();
    }

    public void EnableActionScreen()
    {
        actionScreen.SetActive(true);
    }

    public void DisableActionScreen()
    {
        actionScreen.SetActive(false);
    }

    public void LoadGame()
    {
        // TO DO: Needs to be reworked
        mainMenuScreen.SetActive(false);
        //loadingScreen.SetActive(true);
        gameLogic.StartGame();
        inGameScreen.SetActive(true);
    }
    public void EnableOptionsMenu()
    {
        optionsScreen.SetActive(true);
        mainMenuScreen.SetActive(false);
        if(!gameLogic.gameStarted)
        {
            optionsBackButtontext.text = "Back to Menu";
        }
        else
        {
            optionsBackButtontext.text = "Back to Game";
        }
    }
    public void DisableOptionsMenu()
    {
        if(!gameLogic.gameStarted)
        {
            mainMenuScreen.SetActive(true);
        }
        optionsScreen.SetActive(false);
    }

    public void EnableGameMenuInGame()
    {
        inGameMenu.SetActive(true);
        gameLogic.menuEnabled = true;
    }
    public void DisableGameMenuInGame()
    {
        inGameMenu.SetActive(false);
        gameLogic.menuEnabled = false;
    }

    public void QuitToMainMenu()
    {
        Debug.Log("Quitting to main menu, resetting game");
        gameLogic.gameStarted = false;
    }

    IEnumerator RunLoadingScreenanimation()
    {
        // do the animations animations #endregion
        // start the game via the game logic 
        return null;
    }
    IEnumerator OpenCloseInventory()
    {
        RectTransform inventoryRect = playerInventory.GetComponent<RectTransform>();

        // hardcoding positions, not so good
        Vector2 openPos = new Vector2(0.0f,-25.0f);
        Vector2 closePos = new Vector2(0.0f,25.0f);
        if(playerInventoryOpened)
        {
            //inventoryRect.anchoredPosition = Vector2.Lerp(inventoryRect.anchoredPosition,openPos, step += Time.deltaTime);
            inventoryRect.anchoredPosition = openPos;
        }
        else
        {
            //inventoryRect.anchoredPosition = Vector2.Lerp(inventoryRect.anchoredPosition,closePos, step += Time.deltaTime);
            inventoryRect.anchoredPosition = closePos;
        }
        return null;
    }

    void UpdateInterfaceTexts()
    {
        PlayerBehaviour playerObj = gameLogic.playerObject.GetComponent<PlayerBehaviour>();
        inventoryFoodText.text = ""+playerObj.food;
        inventoryCrewText.text = ""+playerObj.crew;
        inventoryWeaponText.text = ""+playerObj.weapons;
        inventoryLootText.text = ""+playerObj.loot;
    }

    public void ShowNextHint()
    {
        if(m_triviaIndex < hintsAndTrivia.Count-1)
        {
            m_triviaIndex+=1;
        }
        else
        {
           m_triviaIndex = 0;  
        }
        hintsAndTriviaText.text = "Did you know: \n\n"+hintsAndTrivia[m_triviaIndex];
    }
    public void ShowLastHint()
    {
        if(m_triviaIndex > 0)
        {
            m_triviaIndex-=1;
        }
        else
        {
           m_triviaIndex = hintsAndTrivia.Count-1;  
        }
        hintsAndTriviaText.text = "Did you know: \n"+hintsAndTrivia[m_triviaIndex];
    }
}
