﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTileTypes { StartVillageTile, VinlandTile, WaterTile, IslandTile, VillageTile, TraderTile, PirateTile, SerpentTile} // MaelstromTile, SecretTile
public enum Seasons {Summer, Winter}
public class GameTileBehaviour : MonoBehaviour
{
    public GameLogic gameLogic;
    public List<Material> gameTileMaterials = new List<Material>();
    public List<Mesh> gameTileMeshes = new List<Mesh>();
    public List<Mesh> ransackedGameTileMeshes = new List<Mesh>();
    public GameObject gameTileObject;
    public GameTileTypes tileType;
    public bool isStartAreaTile = false;
    public List<Mesh> actionIndicatorMesh = new List<Mesh>();
    public GameObject actionIndicator;

    public GameObject pirateShip;

    public string tileName;
    public int isRansacked = 0;

    private float m_actionIndicatorRotationSpeed = 60.0f;
    void OnEnable()
    {
        DisableActionIndicator();
    }

    void FixedUpdate()
    { 
        actionIndicator.transform.Rotate(Vector3.up, m_actionIndicatorRotationSpeed*Time.deltaTime);
    }
   public void SetTileType(GameTileTypes type)
    {
        tileType = type;
        AdjustTileMaterial();
        AdjustGameTileObjectMesh();
        InitialiseInteractableObject();
    }

    private void AdjustTileMaterial()
    {
        gameObject.GetComponent<Renderer>().material = gameTileMaterials[Random.Range(1,4)];
        switch(tileType)
        {
            case GameTileTypes.StartVillageTile :
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[0];
                gameTileObject.tag="StartVillageTile";
            break;

            case GameTileTypes.WaterTile :
                gameTileObject.SetActive(false);
                 gameTileObject.tag="WaterTile";
            break;

            case GameTileTypes.VillageTile :
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[4];
                gameTileObject.tag="VillageTile";
            break;

            case GameTileTypes.IslandTile :
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[5];
                gameTileObject.tag="IslandTile";
            break;

             case GameTileTypes.TraderTile :
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[9];
                gameTileObject.tag="TraderTile";
            break;

            case GameTileTypes.PirateTile :
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[6];
                gameTileObject.tag="PirateTile";
            break;

            case GameTileTypes.SerpentTile :
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[10];
                gameTileObject.tag="SerpentTile";
            break;

            case GameTileTypes.VinlandTile :
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[7];
                gameTileObject.tag="VinlandTile";
            break;
        }
    }

    void AdjustGameTileObjectMesh()
    {
        MeshFilter obj = gameTileObject.GetComponent<MeshFilter>();
        switch(tileType)
        {
            case GameTileTypes.StartVillageTile :
                obj.mesh = gameTileMeshes[0];
            break;

            case GameTileTypes.VillageTile :
                if( isRansacked == 0 )
                {
                    obj.mesh = gameTileMeshes[1];
                }
                else
                {
                     obj.mesh = ransackedGameTileMeshes[0];
                }
            break;

            case GameTileTypes.IslandTile :
                obj.mesh = gameTileMeshes[2];
            break;

            case GameTileTypes.TraderTile :
                if( isRansacked == 0 )
                {
                    obj.mesh = gameTileMeshes[3];
                }
                else
                {
                     obj.mesh = ransackedGameTileMeshes[1];
                }
            break;

            case GameTileTypes.PirateTile :
                gameTileObject.SetActive(false);
            break;

            case GameTileTypes.SerpentTile :
                obj.mesh = gameTileMeshes[5];
            break;

            case GameTileTypes.VinlandTile :
                obj.mesh = gameTileMeshes[7];
            break;
        }
    }
    void InitialiseInteractableObject()
    {
        switch(tileType)
        {
            case GameTileTypes.StartVillageTile :
                actionIndicator.GetComponent<MeshFilter>().mesh = actionIndicatorMesh[0];
            break;
            
            case GameTileTypes.VinlandTile :
                actionIndicator.GetComponent<MeshFilter>().mesh = actionIndicatorMesh[1];
            break;

            case GameTileTypes.IslandTile :
                actionIndicator.GetComponent<MeshFilter>().mesh = actionIndicatorMesh[2];
            break;

            case GameTileTypes.VillageTile :
                if(isRansacked == 0)
                {
                    actionIndicator.GetComponent<MeshFilter>().mesh = actionIndicatorMesh[3];
                }
                else
                {
                    actionIndicator.GetComponent<MeshFilter>().mesh = actionIndicatorMesh[2];
                }
            break;

            case GameTileTypes.TraderTile :
                 if(isRansacked == 0)
                {
                    actionIndicator.GetComponent<MeshFilter>().mesh = actionIndicatorMesh[3];
                }
                else
                {
                    actionIndicator.GetComponent<MeshFilter>().mesh = actionIndicatorMesh[2];
                }
            break;

            case GameTileTypes.PirateTile :
                actionIndicator.GetComponent<MeshFilter>().mesh = actionIndicatorMesh[4];
            break;

            case GameTileTypes.SerpentTile :
                actionIndicator.GetComponent<MeshFilter>().mesh = actionIndicatorMesh[4];
            break;

            case GameTileTypes.WaterTile :
                actionIndicator.GetComponent<MeshFilter>().mesh = null;
            break;
        }
    }
    public void EnableActionIndicator(Vector3 playerLookDiretion)
    {
        actionIndicator.SetActive(true);
        if(tileType == GameTileTypes.IslandTile && isRansacked == 0 || tileType == GameTileTypes.VillageTile && isRansacked == 1 || tileType == GameTileTypes.TraderTile && isRansacked == 1)
        {
            actionIndicator.transform.rotation = Quaternion.LookRotation(playerLookDiretion,Vector3.up);
        }
    }

    public void DisableActionIndicator()
    {
        actionIndicator.SetActive(false);
    }

    public Vector2 ReturnPosition()
    {
        Vector2 myPosition; 

        myPosition = new Vector2(transform.position.x, transform.position.z);

        return myPosition;
    }

     public void DeserialiseGameTile(string informationToLoad)
    {
        string[] information = informationToLoad.Split(',');

        tileType = (GameTileTypes) System.Enum.Parse( typeof(GameTileTypes), information[0] );
        //Debug.Log(tileType);

        tileName = information[1];

        int.TryParse(information[2], out isRansacked);
        
        SetTileType(tileType);
    }

    public string SerialiseGameTile()
    {
        string infoToSerialise ="";

        infoToSerialise = ""+ tileType.ToString()+","+tileName+","+isRansacked;
        return infoToSerialise;
    }

    public void DespawnPirate()
    {
        if(pirateShip != null)
        {
            PirateShipBehaviour pirate = pirateShip.GetComponent<PirateShipBehaviour>();

            if(pirate.sightedPrey)
            {
                // will not despawn, turn tile into tiletype watertile instead
                tileType = GameTileTypes.WaterTile;
                pirateShip  = null;
            }
            else
            {
                gameLogic.ReturnPirateToPool(pirateShip);
                pirateShip  = null;
            }
        }
    }
}
