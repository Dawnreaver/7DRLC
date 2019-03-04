using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTileTypes { StartVillageTile,VinlandTile, WaterTile, IslandTile, VillageTile, DangerTile, SecretTile}
public enum Seasons {Summer, Winter}
public class GameTileBehaviour : MonoBehaviour
{
    public List<Material> gameTileMaterials = new List<Material>();
    public List<Mesh> gameTileMeshes = new List<Mesh>();
    string[] m_monthsOfTheVikingYear = new string[]
    {
        "Einmánuður",
        "Gói",
        "Gormánuður",
        "Harpa",
        "Haustmánuður",
        "Heyannir",
        "Mörsugur",
        "Skerpla",
        "Sólmánuður",
        "Þorri / Thorri",
        "Tvímánuður",
        "Ýlir"

    };
    public GameTileTypes tileType;
    public bool isStartAreaTile = false;
    public bool canBeSeenByPlayer = false;

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
            break;

            case GameTileTypes.WaterTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[Random.Range(1,4)];
            break;

            case GameTileTypes.VillageTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[4];
            break;

            case GameTileTypes.IslandTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[5];
            break;

            case GameTileTypes.DangerTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[6];
            break;

            case GameTileTypes.VinlandTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[7];
            break;
            case GameTileTypes.SecretTile :
                gameObject.GetComponent<Renderer>().material = gameTileMaterials[7];
            break;
        }
    }

    private void AdjustTileMesh()
    {

    }

    public void CheckPlayervisibility( GameObject player)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if(distanceToPlayer > 1.5f)
        {
            canBeSeenByPlayer = false;
            // remove from list of visible tiles
            // add back to tile pool
        }
        else if(distanceToPlayer <= 1.5f)
        {
            canBeSeenByPlayer = true;
            // anything else we want to do when the tile can be seen 
        }
    }
}
