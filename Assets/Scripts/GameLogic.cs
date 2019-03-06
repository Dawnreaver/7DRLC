using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
// map generation

    // game tiles
    public List<GameObject> gameTilesPool = new List<GameObject>();
    public List<GameObject> usedGameTiles = new List<GameObject>();
    private List<GameObject> m_gameTilesStartArea = new List<GameObject>();
    private int m_gameTilesPoolSize = 48;
    public GameObject gameTilePrefab;
    
    // start position
    public bool randomizeStartPosition = false;

    public Vector3 gameTileStartPosition = new Vector3(10,0,0);
    public float gameTileStartPositionHeight = -0.75f;
    public int minX = 2, maxX = 3, minZ = 2, maxZ = 3;
    // Fog
    public List<GameObject> fogObjectsPool = new List<GameObject>();
    public GameObject fogPrefab;
    List<Vector2> fogPosition = new List<Vector2>();
    private int m_fogObjectsPoolSize = 42;
// game play
    //player
    public GameObject playerObjectPrefab;
    public GameObject playerObject;
    private Vector3 m_playerPosition;
    private Vector3 m_oldPlayerPosition;

    // Camera
    public MainCameraBehaviour mainCamera;
    public float cameraMOvementSpeed = 5.0f;

    // game logic
    public bool gameStarted = false;
    public bool menuEnabled = false;
    public int numberOfNewSpawnedTiles = 0;
    public int vinlandTileThreshold = 100;
    public bool vinlandTileHasSpawned = false;
    public int secretTileTreshold =50;
    public bool secretTileHasSpawned = false;

    Dictionary<Vector2, GameTileTypes> gameTilesDictionary = new Dictionary<Vector2, GameTileTypes>();
    // Start is called before the first frame update
    void Start()
    {
        if(randomizeStartPosition)
        {
            gameTileStartPosition = new Vector3((int)Random.Range(-50,50), 0,(int)Random.Range(-50,50));
        }
        PoolGameTiles();
        PoolFogObjects();
        SpawnStartArea(gameTileStartPosition);
        SpawnPlayer();
        mainCamera.SetCameraMovementSpeed(cameraMOvementSpeed);
        ApplyFog();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_playerPosition = playerObject.transform.position;
        if(m_playerPosition != m_oldPlayerPosition)
        {
            m_oldPlayerPosition = m_playerPosition;
            SpawnAndDespawnBoardTiles();
        }

    }

    void OnDrawGizmos()
    {
    }

    void PoolGameTiles()
    {
        for( int a = 0; a < m_gameTilesPoolSize; a ++)
        {
            GameObject boardTile = Instantiate(gameTilePrefab, Vector3.zero, Quaternion.identity) as GameObject;
            boardTile.name = "boardTile";
            gameTilesPool.Add(boardTile);
            boardTile.transform.SetParent(gameObject.transform);
            boardTile.SetActive(false);
        }
    }
    void PoolFogObjects()
    {
        for( int a = 0; a < m_fogObjectsPoolSize; a ++)
        {
            GameObject fogObject = Instantiate(fogPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            fogObject.name = "fogObject";
            fogObjectsPool.Add(fogObject);
            fogObject.transform.SetParent(gameObject.transform);
            fogObject.SetActive(false);
        }
    }

    void SpawnStartArea(Vector3 mainVillageStartPosition)
    {
        GameObject tile;
    /*
         x x x
       x x x x x
       x x o x x
       x x x x x
         x x x 
    */

        for(int x = (int)mainVillageStartPosition.x-minX; x < (int)mainVillageStartPosition.x+maxX; x++)
        {
            for(int z = (int)mainVillageStartPosition.z-minZ; z < (int)mainVillageStartPosition.z+maxZ; z++)
            {
                if( x == mainVillageStartPosition.x+maxX-1 && z == mainVillageStartPosition.z-minZ || 
                    x == mainVillageStartPosition.x-minX && z == mainVillageStartPosition.z+maxZ-1 || 
                    x == mainVillageStartPosition.x-minX && z == mainVillageStartPosition.z-minZ || 
                    x == mainVillageStartPosition.x+maxX-1 && z == mainVillageStartPosition.z+maxZ-1)
                {
                    //Debug.Log("Corner: "+x+","+z);
                }
                else{
                    tile = gameTilesPool[gameTilesPool.Count-1];
                    tile.transform.position = new Vector3(x,gameTileStartPositionHeight,z);
                    GameTileBehaviour gameTile = tile.GetComponent<GameTileBehaviour>();
                    gameTile.isStartAreaTile = true;
                    if(x == (int)mainVillageStartPosition.x && z == (int)mainVillageStartPosition.z)
                    {
                        tile.name = "Start Village";
                        gameTile.SetTileType(GameTileTypes.StartVillageTile);
                    }
                    else
                    {
                        tile.name = "GameTile";
                        gameTile.SetTileType(GameTileTypes.WaterTile);
                    }
                    tile.SetActive(true);
                    m_gameTilesStartArea.Add(tile);

                    gameTilesPool.Remove(tile);
                    // add tiles to the directory, with their tile type
                    gameTilesDictionary.Add(new Vector2(tile.transform.position.x,tile.transform.position.z),tile.GetComponent<GameTileBehaviour>().tileType);
                }
            }
        }
    }
    void SpawnPlayer()
        {
            playerObject = Instantiate(playerObjectPrefab,transform.position, Quaternion.identity) as GameObject;
            playerObject.name = "PlayerObject";
            playerObject.transform.position = gameTileStartPosition;
            mainCamera.followObject = playerObject;
            mainCamera.SetStartPosition();
            playerObject.GetComponent<PlayerBehaviour>().gameLogic = this;

            // perform any actions to randomise the playe
            playerObject.GetComponent<PlayerBehaviour>().InitialisePlayer();
            // actions defined in player script? 
        }
    void SpawnAndDespawnBoardTiles()
    { 
        /*
            x x x 
            x o x 
            x x x
        */

        List<GameObject> tilesToRemove = new List<GameObject>();
        // Get the positions sorrounding the player #endregion [x]
        List<Vector3> potentialSpawnPositions = new List<Vector3>(){
            new Vector3(m_playerPosition.x, gameTileStartPositionHeight, m_playerPosition.z),
            new Vector3(m_playerPosition.x+1,gameTileStartPositionHeight,m_playerPosition.z),
            new Vector3(m_playerPosition.x+1,gameTileStartPositionHeight,m_playerPosition.z+1),
            new Vector3(m_playerPosition.x+1,gameTileStartPositionHeight,m_playerPosition.z-1),
            new Vector3(m_playerPosition.x-1,gameTileStartPositionHeight,m_playerPosition.z+1),
            new Vector3(m_playerPosition.x-1,gameTileStartPositionHeight,m_playerPosition.z-1),
            new Vector3(m_playerPosition.x,gameTileStartPositionHeight,m_playerPosition.z+1),
            new Vector3(m_playerPosition.x,gameTileStartPositionHeight,m_playerPosition.z-1),
            new Vector3(m_playerPosition.x-1,gameTileStartPositionHeight,m_playerPosition.z)
            };
            // remove used tiles that are no longer needed
            for(int a = 0; a < usedGameTiles.Count; a++)
            {
                if(!potentialSpawnPositions.Contains(usedGameTiles[a].transform.position))
                {
                    ReturnGameTileToPool(usedGameTiles[a]);
                    a--;
                }
            }
            // spwan new tiles / tiles that we already visited
            for( int b = 0; b < potentialSpawnPositions.Count; b++)
            {
                GameTileTypes type;
                GameObject tile;

                if(!gameTilesDictionary.TryGetValue(new Vector2(potentialSpawnPositions[b].x,potentialSpawnPositions[b].z), out type))
                {
                    //Debug.Log("Have to creaye new tile...");
                    if(gameTilesPool.Count >= 1)
                    {
                        tile = gameTilesPool[gameTilesPool.Count-1];
                        tile.transform.position = potentialSpawnPositions[b];
                        GameTileTypes tileType = ChooseRandomGameTile();
                        tile.GetComponent<GameTileBehaviour>().SetTileType(tileType);
                        tile.SetActive(true);
                        gameTilesDictionary.Add(new Vector2(potentialSpawnPositions[b].x,potentialSpawnPositions[b].z),tileType);
                        gameTilesPool.Remove(tile);
                        usedGameTiles.Add(tile);
                        numberOfNewSpawnedTiles += 1;
                    }
                    else
                    {
                        Debug.Log("Out of game tiles...");
                    }
                }
                else if(gameTilesDictionary.TryGetValue(new Vector2(potentialSpawnPositions[b].x,potentialSpawnPositions[b].z), out type) && !IsTileInUse(potentialSpawnPositions[b]) && !IsTileNotAStartTile(potentialSpawnPositions[b]))
                {
                    Debug.Log("Tile is part of the dictionary, but needs to be re-spawned");
                    if(gameTilesPool.Count >= 1)
                   {
                       tile = gameTilesPool[gameTilesPool.Count-1];
                       tile.transform.position = potentialSpawnPositions[b];
                       tile.GetComponent<GameTileBehaviour>().SetTileType(type);
                       tile.SetActive(true);
                       gameTilesPool.Remove(tile);
                       usedGameTiles.Add(tile);
                   }
                   else
                   {
                       Debug.Log("Out of game tiles...");
                   }
                }
                else
                {
                    //Debug.Log("it's all good man");
                }
            }
        ApplyFog();
    }
    void ApplyFog()
    {
        for(int x = (int) gameTileStartPosition.x-3; x < (int)gameTileStartPosition.x+4; x++)
        {
            for(int z = (int) gameTileStartPosition.z-3; z < (int)gameTileStartPosition.z+4; z++)
            {
                if( x == gameTileStartPosition.x+3 && z == gameTileStartPosition.z-3 || 
                    x == gameTileStartPosition.x-3 && z == gameTileStartPosition.z+3 || 
                    x == gameTileStartPosition.x-3 && z == gameTileStartPosition.z-3 || 
                    x == gameTileStartPosition.x+3 && z == gameTileStartPosition.z+3)
                {
                    //Debug.Log("Corner: "+x+","+z);
                }
                else{
                    Vector2 newPos = new Vector2(x,z);
                    fogPosition.Add(newPos);
                }
            }
        }
        for(int a = (int)playerObject.transform.position.x-2; a < (int)playerObject.transform.position.x+3; a++)
        {
            for(int b = (int)playerObject.transform.position.z-2; b < (int)playerObject.transform.position.z+3; b++)
            {
                Vector2 newPos = new Vector2(a,b);
                if(!fogPosition.Contains(newPos))
                {
                    fogPosition.Add(newPos);
                }
            }
        }
        for(int c = 0; c < usedGameTiles.Count; c++)
        {
            Vector2 newPos = new Vector2(usedGameTiles[c].transform.position.x,usedGameTiles[c].transform.position.z);
            if(fogPosition.Contains(newPos))
            {
                fogPosition.Remove(newPos);
            }
        }
        for(int d = 0; d < m_gameTilesStartArea.Count; d++)
        {
            Vector2 newPos = new Vector2(m_gameTilesStartArea[d].transform.position.x,m_gameTilesStartArea[d].transform.position.z);
            if(fogPosition.Contains(newPos))
            {
                fogPosition.Remove(newPos);
            }
        }
        for(int e = 0; e < fogObjectsPool.Count; e++)
        {
            if(e < fogPosition.Count)
            {
                fogObjectsPool[e].transform.position = new Vector3(fogPosition[e].x, 0.0f,fogPosition[e].y);
                fogObjectsPool[e].SetActive(true);
            }
            else
            {
                fogObjectsPool[e].transform.position = Vector3.zero;
                fogObjectsPool[e].SetActive(false);
            }
        }

        fogPosition.Clear();
    }

    void ReturnGameTileToPool(GameObject tile)
    {
        tile.transform.position = transform.position;
        tile.SetActive(false);
        usedGameTiles.Remove(tile);
        gameTilesPool.Add(tile);
    }

    bool IsTileInUse(Vector3 position)
    {
        bool isInUse = false;
        foreach(GameObject gameTile in usedGameTiles)
        {
            if(gameTile.transform.position == position)
            {
                isInUse = true;
                break;
            }
        }

        return isInUse;
    }

    bool IsTileNotAStartTile(Vector3 position)
    {
        bool isStartTile = false;
        foreach(GameObject gameTile in m_gameTilesStartArea)
        {
            if(gameTile.transform.position == position)
            {
                isStartTile = true;
                break;
            }
        }

        return isStartTile;
    }

    public GameTileTypes ChooseRandomGameTile()
    {
        GameTileTypes randomTile;

        float randomValue;
        
        randomValue = Random.Range(0.0f,1.0f);
        //Debug.Log("randomValue: "+randomValue);

        if(randomValue <= 0.85f)
        {
            randomTile = GameTileTypes.WaterTile;
        }
        else
        {
            randomTile = (GameTileTypes) Random.Range(4,7); // Excluded - 1: StartVillageTile 2: VinlandTile 3: WaterTile 7: SecretTile
        }

        return randomTile;
    }
}