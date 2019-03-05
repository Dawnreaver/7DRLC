using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTileTypes { StartVillageTile,VinlandTile, WaterTile, IslandTile, VillageTile, DangerTile, SecretTile}
public enum Seasons {Summer, Winter}
public class GameTileBehaviour : MonoBehaviour
{
    public List<Material> gameTileMaterials = new List<Material>();
    public List<Mesh> gameTileMeshes = new List<Mesh>();

    public GameObject gameTileObject;

    public GameTileTypes tileType;
    public bool isStartAreaTile = false;

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
    }

   public void SetTileType(GameTileTypes type)
    {
        tileType = type;
        AdjustTileMaterial();
        AdjustTileMesh();
    }

    private void AdjustTileMaterial()
    {
        switch(tileType)
        {
            case GameTileTypes.StartVillageTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[0];
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[0];
                gameTileObject.tag="Home";
            break;

            case GameTileTypes.WaterTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[Random.Range(1,4)];
                gameTileObject.SetActive(false);
            break;

            case GameTileTypes.VillageTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[4];
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[4];
                gameTileObject.tag="Village";
            break;

            case GameTileTypes.IslandTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[5];
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[5];
                gameTileObject.tag="Island";
            break;

            case GameTileTypes.DangerTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[6];
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[6];
            break;

            case GameTileTypes.VinlandTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[7];
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[7];
                gameTileObject.tag="Vinland";
            break;
            case GameTileTypes.SecretTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[8];
                gameTileObject.SetActive(true);
                gameTileObject.GetComponent<Renderer>().material = gameTileMaterials[8];
                gameTileObject.tag="Fortress";
            break;
        }
    }

    private void AdjustTileMesh()
    {
        
    }

}
