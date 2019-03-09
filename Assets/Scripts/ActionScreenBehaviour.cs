﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionScreenBehaviour : MonoBehaviour
{
    public GameLogic gameLogic;
    public InterfaceBehaviour gameInterface;
    private PlayerBehaviour m_playerBehaviour;
    
    public Text tileName;
    public Text name;
// Action buttons
    // home village 
    public GameObject ButtonRetire;
    public GameObject ButtonResume;
    // Trader
    public GameObject ButtonTradeWeapon;
    // Village 
    public GameObject ButtonTradeFood;
    // Ransack 
    public GameObject ButtonRansack;

    void FixedUpdate()
    {
        if(m_playerBehaviour == null)
        {
            m_playerBehaviour = gameLogic.playerObject.GetComponent<PlayerBehaviour>();
        }
        if(m_playerBehaviour.loot < 1)
        {
            ButtonTradeFood.GetComponent<Button>().interactable = false;
            ButtonTradeWeapon.GetComponent<Button>().interactable = false;
        }
        else if(m_playerBehaviour.loot > 1 && m_playerBehaviour.loot < 5)
        {
            ButtonTradeFood.GetComponent<Button>().interactable = true;
            ButtonTradeWeapon.GetComponent<Button>().interactable = false;
        }
        else
        {
            ButtonTradeFood.GetComponent<Button>().interactable = true;
            ButtonTradeWeapon.GetComponent<Button>().interactable = true;
        }
    }

    public void AdjustActionDialogue(GameTileTypes m_gameTile, string nameString = "")
    {
        ResetActions();

        switch(m_gameTile)
        {
            case GameTileTypes.VillageTile :
                name.text = nameString;
                tileName.text ="- Village -";
                ButtonTradeFood.SetActive(true);
                ButtonRansack.SetActive(true);
            break;

            case GameTileTypes.TraderTile :
                name.text = nameString;
                tileName.text ="- Trader -";
                ButtonTradeWeapon.SetActive(true);
                ButtonRansack.SetActive(true);
            break;

            case GameTileTypes.StartVillageTile :
                name.text = nameString;
                tileName.text ="- Home Village -";
                ButtonRetire.SetActive(true);
                ButtonResume.SetActive(true);
            break;
        }
    }

    public void TradeFood()
    {
        gameInterface.DisableActionScreen();
        gameLogic.TradeFood();
    }
    public void TradeWeapon()
    {
        gameInterface.DisableActionScreen();
        gameLogic.TradeWeapon();
    }
    public void RansackGameTile()
    {
        gameInterface.DisableActionScreen();
        gameLogic.RansackGameTile();
    }
    public void RetireFromAdventure()
    {
        gameInterface.DisableActionScreen();
        gameLogic.RetireFromAdventure();
    }
    public void ResumeAdventure()
    {
        gameInterface.DisableActionScreen();
    }

    void ResetActions()
    {
        ButtonRetire.SetActive(false);
        ButtonResume.SetActive(false);
        ButtonTradeWeapon.SetActive(false);
        ButtonTradeFood.SetActive(false);
        ButtonRansack.SetActive(false);
    }

}