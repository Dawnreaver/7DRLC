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
    // Start is called before the first frame update
    void Start()
    {
        // Splash screen
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
        }
    }

    void FixedUpdate()
    {
        UpdateInterfaceTexts();
    }

    public void LoadGame()
    {
        mainMenuScreen.SetActive(false);
        loadingScreen.SetActive(true);
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
}
