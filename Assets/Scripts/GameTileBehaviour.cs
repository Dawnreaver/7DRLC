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
        if(actionIndicator != null)
        {
            actionIndicator.transform.Rotate(Vector3.up, m_actionIndicatorRotationSpeed*Time.deltaTime);
        }        
    }
   public void SetTileType(GameTileTypes type)
    {
        tileType = type;
        AdjustTileMaterial();
        //AdjustTileMesh();
        InitialiseInteractableObject();
    }

    private void AdjustTileMaterial()
    {
        switch(tileType)
        {
            case GameTileTypes.StartVillageTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[0];
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[0];
                gameTileObject.tag="StartVillageTile";
            break;

            case GameTileTypes.WaterTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[Random.Range(1,4)];
                gameTileObject.SetActive(false);
                 gameTileObject.tag="WaterTile";
            break;

            case GameTileTypes.VillageTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[4];
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[4];
                gameTileObject.tag="VillageTile";
            break;

            case GameTileTypes.IslandTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[5];
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[5];
                gameTileObject.tag="IslandTile";
            break;

             case GameTileTypes.TraderTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[8];
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[8];
                gameTileObject.tag="TraderTile";
            break;

            case GameTileTypes.PirateTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[6];
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[6];
                gameTileObject.tag="PirateTile";
            break;

            case GameTileTypes.SerpentTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[6];
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[6];
                 gameTileObject.tag="SerpentTile";
            break;

            case GameTileTypes.MaelstromTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[6];
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[6];
                 gameTileObject.tag="MaelstromTile";
            break;

            case GameTileTypes.VinlandTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[7];
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[7];
                gameTileObject.tag="VinlandTile";
            break;
            case GameTileTypes.SecretTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[8];
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[8];
                gameTileObject.tag="SecretTile";
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

    public void EnableActionIndicator()
    {
        actionIndicator.SetActive(true);
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
