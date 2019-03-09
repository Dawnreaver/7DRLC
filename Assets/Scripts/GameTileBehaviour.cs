using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTileTypes { StartVillageTile, VinlandTile, WaterTile, IslandTile, VillageTile, TraderTile, PirateTile, SerpentTile,MaelstromTile, SecretTile}
public enum Seasons {Summer, Winter}
public class GameTileBehaviour : MonoBehaviour
{
    public List<Material> gameTileMaterials = new List<Material>();
    public List<Mesh> gameTileMeshes = new List<Mesh>();
    public GameObject gameTileObject;
    public GameTileTypes tileType;
    public bool isStartAreaTile = false;
    public List<Mesh> actionIndicatorMesh = new List<Mesh>();
    public GameObject actionIndicator;

    public string name;

    private float m_actionIndicatorRotationSpeed = 60.0f;
    void OnEnable()
    {
        if(isStartAreaTile)
        {
            gameObject.name = "StartTile "+transform.position.x+"/"+transform.position.z;
        }
        else
        {
            gameObject.name = "GameTile "+transform.position.x+"/"+transform.position.z;
        }

        DisableActionIndicator();
    }

    void FixedUpdate()
    { 
        if(actionIndicator != null && tileType == GameTileTypes.VinlandTile || actionIndicator != null && tileType == GameTileTypes.StartVillageTile || actionIndicator != null && tileType == GameTileTypes.SecretTile)
        {
            actionIndicator.transform.Rotate(Vector3.up, m_actionIndicatorRotationSpeed*Time.deltaTime);
        }        
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
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[6];
                 gameTileObject.tag="SerpentTile";
            break;

            case GameTileTypes.MaelstromTile :
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[6];
                 gameTileObject.tag="MaelstromTile";
            break;

            case GameTileTypes.VinlandTile :
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[7];
                gameTileObject.tag="VinlandTile";
            break;
            case GameTileTypes.SecretTile :
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[8];
                gameTileObject.tag="SecretTile";
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

            /*case GameTileTypes.WaterTile :
                
            break; */

            case GameTileTypes.VillageTile :
                obj.mesh = gameTileMeshes[1];
            break;

            case GameTileTypes.IslandTile :
                obj.mesh = gameTileMeshes[2];
            break;

            case GameTileTypes.TraderTile :
                obj.mesh = gameTileMeshes[3];
            break;

            case GameTileTypes.PirateTile :
                obj.mesh = gameTileMeshes[4];
            break;

            case GameTileTypes.SerpentTile :
                obj.mesh = gameTileMeshes[5];
            break;

            case GameTileTypes.MaelstromTile :
                obj.mesh = gameTileMeshes[6];
            break;

            case GameTileTypes.VinlandTile :
                obj.mesh = gameTileMeshes[7];
            break;
            case GameTileTypes.SecretTile :
                obj.mesh = gameTileMeshes[8];
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
                actionIndicator.GetComponent<MeshFilter>().mesh = actionIndicatorMesh[3];
            break;

            case GameTileTypes.TraderTile :
                actionIndicator.GetComponent<MeshFilter>().mesh = actionIndicatorMesh[3];
            break;

            case GameTileTypes.PirateTile :
                actionIndicator.GetComponent<MeshFilter>().mesh = actionIndicatorMesh[4];
            break;

            case GameTileTypes.SerpentTile :
                actionIndicator.GetComponent<MeshFilter>().mesh = actionIndicatorMesh[4];
            break;

            case GameTileTypes.SecretTile :
                actionIndicator.GetComponent<MeshFilter>().mesh = actionIndicatorMesh[5];
            break;

            case GameTileTypes.WaterTile :
                actionIndicator.GetComponent<MeshFilter>().mesh = null;
            break;

            case GameTileTypes.MaelstromTile :
                actionIndicator.GetComponent<MeshFilter>().mesh = null;
            break;
        }
    }
    public void EnableActionIndicator(Vector3 playerLookDiretion)
    {
        actionIndicator.SetActive(true);
        if(tileType == GameTileTypes.IslandTile)
        {
            actionIndicator.transform.rotation = Quaternion.LookRotation(playerLookDiretion,Vector3.up);
        }
        else if(tileType == GameTileTypes.VillageTile || tileType == GameTileTypes.TraderTile)
        {
            actionIndicator.transform.rotation = Quaternion.LookRotation(Vector3.right,Vector3.up);
        }
        else if(tileType == GameTileTypes.PirateTile || tileType == GameTileTypes.SerpentTile)
        {
            actionIndicator.transform.rotation = Quaternion.LookRotation(Vector3.back,Vector3.up);
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
}
