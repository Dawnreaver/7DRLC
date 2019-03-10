using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public InterfaceBehaviour gameInterface;
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

    public int playerStartFood = 48;
    public int playerStartLoot = 5;

    // Camera
    public MainCameraBehaviour mainCamera;
    public float cameraMOvementSpeed = 5.0f;

    // game logic
    public bool gameStarted = false;
    public bool menuEnabled = false;
    public int numberOfNewSpawnedTiles = 0;
    public int vinlandTileThreshold = 150;
    public bool vinlandTileHasSpawned = false;

    public int uneventfulJourneyCount = 0;

    List<string> m_villageNames = new List<string>()
    {
        "Akrar",
        "Arnallsstaoir",
        "Asbjarnarstaoir",
        "Atley",
        "Ballara",
        "Belgsdalr",
        "Blaskogsa",
        "Brenna",
        "Brunastaoir",
        "Brynjudalr",
        "Eyjafjoll",
        "Fagradalsa",
        "Flokadalr",
        "Forsoeludalr",
        "Galmastrond",
        "Glera nyrori",
        "Gonguskarosaross",
        "Grenivik",
        "Gufaross",
        "Hernar",
        "Hlioarendi",
        "Hlioir",
        "Hnjoska",
        "Holtastaoir",
        "Horgardalsa",
        "Hrafnstoptir",
        "Hraungeroi",
        "Hvaleyrr",
        "Hvalvatnsfjoror",
        "Hvanndalir",
        "Kalmanstunga",
        "Karlsdalr",
        "Kerlingarfjoror",
        "Kiojaleit",
        "Kirkjubolstaor",
        "Krofluhellir",
        "Krossa",
        "Lagarfljotsstrandir",
        "Laugardalr",
        "Lonlond",
        "Njarovik",
        "Oleifsbjorg",
        "Ormsstaoir",
        "Osomi",
        "Raumsdaelafylki",
        "Rauoaloekr",
        "Sauoafellslond",
        "Seljasund"
    };

    // Saga generation
    public SaveSaga sagaLogic;

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

    public void StartGame()
    {
        gameStarted = true;
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
                        gameTile.name = ReturnRandomString(m_villageNames);
                    }
                    /* else if(x == 2+ mainVillageStartPosition.x && z == (int)mainVillageStartPosition.z)
                    {
                        gameTile.SetTileType(GameTileTypes.VinlandTile);
                    }*/
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
            PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();

            int randomPlayerPosition = Random.Range(0,4);

            if(randomPlayerPosition == 0)
            {
                playerObject.transform.position = new Vector3(gameTileStartPosition.x,0.0f,gameTileStartPosition.z+1);
                playerObject.transform.rotation = Quaternion.LookRotation(Vector3.forward,Vector3.up);
            }
            else if(randomPlayerPosition == 1)
            {
                playerObject.transform.position = new Vector3(gameTileStartPosition.x+1,0.0f,gameTileStartPosition.z);
                playerObject.transform.rotation = Quaternion.LookRotation(Vector3.right,Vector3.up);
            }
            else if(randomPlayerPosition == 2)
            {
                playerObject.transform.position = new Vector3(gameTileStartPosition.x,0.0f,gameTileStartPosition.z-1);
                playerObject.transform.rotation = Quaternion.LookRotation(Vector3.back,Vector3.up);
            }
            else
            {
                playerObject.transform.position = new Vector3(gameTileStartPosition.x-1,0.0f,gameTileStartPosition.z);
                playerObject.transform.rotation = Quaternion.LookRotation(Vector3.left,Vector3.up);
            }
            
            mainCamera.followObject = playerObject;
            mainCamera.SetStartPosition();
            obj.gameLogic = this;
            obj.startFood = playerStartFood;
            obj.startLoot = playerStartLoot;
            // perform any actions to randomise the playe
            obj.InitialisePlayer();
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
                        if(tileType == GameTileTypes.SerpentTile || tileType == GameTileTypes.PirateTile)
                        {
                            TurnGameTileRandomly(tile);
                        }
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
                    //Debug.Log("Tile is part of the dictionary, but needs to be re-spawned");
                    if(gameTilesPool.Count >= 1)
                   {
                        tile = gameTilesPool[gameTilesPool.Count-1];
                        tile.transform.position = potentialSpawnPositions[b];
                        tile.GetComponent<GameTileBehaviour>().SetTileType(type);
                        if(type == GameTileTypes.SerpentTile || type == GameTileTypes.PirateTile)
                        {
                            TurnGameTileRandomly(tile);
                        }
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
        GameTileTypes randomTile = GameTileTypes.WaterTile;

        float randomValue;
        float randomEventTile;
        
        randomValue = Random.Range(0.0f,1.0f);
        //Debug.Log("randomValue: "+randomValue);
        randomEventTile = Random.Range(0.0f,1.0f);

        if(randomValue <= 0.85f)
        {
            randomTile = GameTileTypes.WaterTile;
        }
        else
        {
            if(randomEventTile <= 0.20f)
            {
                if(!vinlandTileHasSpawned && numberOfNewSpawnedTiles < vinlandTileThreshold )
                {
                    randomTile = GameTileTypes.IslandTile;
                }
                else if(! vinlandTileHasSpawned && numberOfNewSpawnedTiles >= vinlandTileThreshold)
                {
                     randomTile = GameTileTypes.VinlandTile;
                     vinlandTileHasSpawned = true;
                }
                else if(vinlandTileHasSpawned && numberOfNewSpawnedTiles >= vinlandTileThreshold)
                {
                    randomTile = GameTileTypes.IslandTile;
                }
            }
            else if(randomEventTile >0.20f && randomEventTile <= 0.40f)
            {
                randomTile = GameTileTypes.VillageTile;
            }
            else if(randomEventTile > 0.40f && randomEventTile <= 0.60f)
            {
                randomTile = GameTileTypes.TraderTile;
            }
            else if(randomEventTile > 0.60f && randomEventTile <= 0.80f)
            {
                randomTile = GameTileTypes.PirateTile;
            }
            else if(randomEventTile > 0.80f && randomEventTile <= 1.0f)
            {
                randomTile = GameTileTypes.SerpentTile;
            }
        }
        return randomTile;
    }

    void TurnGameTileRandomly(GameObject gameTile)
    {
        int randomGameTileRotation = Random.Range(0,4);

            if(randomGameTileRotation == 0)
            {
                gameTile.transform.rotation = Quaternion.LookRotation(Vector3.forward,Vector3.up);
            }
            else if(randomGameTileRotation == 1)
            {
                gameTile.transform.rotation = Quaternion.LookRotation(Vector3.right,Vector3.up);
            }
            else if(randomGameTileRotation == 2)
            {
                gameTile.transform.rotation = Quaternion.LookRotation(Vector3.back,Vector3.up);
            }
            else
            {
                gameTile.transform.rotation = Quaternion.LookRotation(Vector3.left,Vector3.up);
            }
    }

     public string ReturnRandomString(List<string> strings, string unique = null)
    {
        string randomString;

        randomString = strings[Random.Range(0,strings.Count)];

        if(unique != null)
        {
            string uniqueName;
            bool generateNewName = true;

            while(generateNewName)
            {
                randomString = strings[Random.Range(0,strings.Count)];
                if(randomString != unique)
                {
                    generateNewName = false;

                    uniqueName = randomString;
                    return uniqueName;
                }
            }
        }
        return randomString;
    }

    public string ReturnRandomString(string[] strings, string unique = null)
    {
        string randomString;

        randomString = strings[Random.Range(0,strings.Length)];

        if(unique != null)
        {
            string uniqueName;
            bool generateNewName = true;

            while(generateNewName)
            {
                randomString = strings[Random.Range(0,strings.Length)];
                if(randomString != unique)
                {
                    generateNewName = false;

                    uniqueName = randomString;
                    return uniqueName;
                }
            }
        }
        return randomString;
    }
    public void RetireFromAdventure()
    {
        GenerateEndGameEarly();
        
    }

    public void WinGame()
    {
        GenerateFoundVinland();
        sagaLogic.AddSagaEnd();
        menuEnabled = true;
        gameInterface.EnableEndGameScreenWin();
    }
    public void LoseGame()
    {
        sagaLogic.AddSagaEnd();
        menuEnabled = true;
        gameInterface.EnableEndGameScreenLose();
    }

    public void AttackGameTile(GameObject gameTile)
    {
        PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();
        GameTileBehaviour tile = gameTile.GetComponent<GameTileBehaviour>();
        if(tile.tileType == GameTileTypes.PirateTile)
        {
            if(obj.weapons == 0)
            {
                obj.crew -= Random.Range(20,31);
                if( obj.crew < 0)
                {
                    obj.crew = 0; 
                    GenerateEndFailedPirate();
                    LoseGame();
                    return;
                }
                else
                {
                    PurgeGameTile(tile, GameTileTypes.WaterTile);
                    obj.loot += Random.Range(3,10);
                    obj.food += Random.Range(1,4);
                    GenerateBeatPirate();
                }
            }
            else if(obj.weapons > 0)
            {
                obj.weapons -= 1;
                obj.crew -= Random.Range(1,4);
                if( obj.crew < 0)
                {
                    obj.crew = 0; 
                    GenerateEndFailedPirate();
                    LoseGame();
                    return;
                }
                else
                {
                    PurgeGameTile(tile, GameTileTypes.WaterTile);
                    obj.loot += Random.Range(3,10);
                    obj.food += Random.Range(1,4);
                    GenerateBeatPirate();
                }

            }
        }

        if(tile.tileType == GameTileTypes.SerpentTile)
        {
            if(obj.weapons == 0)
            {
                GenerateEndFailedSerpent();
                LoseGame();
                return;
            }
            else
            {
                int noOfWeapons = obj.weapons;
                obj.weapons = 0;

                PurgeGameTile(tile, GameTileTypes.WaterTile);
                obj.loot += (noOfWeapons*2)+Random.Range(1,4);
                GenerateMovedSerpent();
            }
        }

    }

    public void PurgeGameTile(GameTileBehaviour tileToPurge, GameTileTypes type)
    {
        gameTilesDictionary.Remove(tileToPurge.ReturnPosition());
        tileToPurge.SetTileType (type);
        gameTilesDictionary.Add(tileToPurge.ReturnPosition(),type);
    }

    public void RansackGameTile()
    {
        PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();
        GameTileBehaviour tile = obj.lastLookedAtObject.GetComponent<GameTileBehaviour>();
        Debug.Log("Burn it to the ground!");

        // lose a small number of men for the attack if you don't have weapons
        if(obj.weapons == 0)
        {
            obj.crew -= Random.Range(1,3);
            if(obj.crew < 0)
            {
                obj.crew = 0;

                if(tile.tileType == GameTileTypes.TraderTile)
                {
                    GenerateEndFailedRansack();
                    Debug.Log("Died while rading a trader...");
                    LoseGame();
                    return;
                }
                else if(tile.tileType == GameTileTypes.VillageTile)
                {
                    GenerateEndFailedRansack();
                    Debug.Log("Died while rading a village...");
                    LoseGame();
                    return;
                }
            }
        }
        else
        {
            obj.weapons -= 1; 
        }
        if(tile.tileType == GameTileTypes.TraderTile)
        {
            obj.loot += 1+ Random.Range(1,4);
            //obj.weapons += 1+ Random.Range(1,3);
            Debug.Log("Raided a trader...");
            GenerateRaidedTrader();

            // play effect
        }
        else if(tile.tileType == GameTileTypes.VillageTile)
        {
            obj.loot += 1+ Random.Range(0,3);
            obj.food += 1+ Random.Range(1,4);
            Debug.Log("Raided a village...");
            GenerateRaidedVillage();

            // play effect
        }
        // turn ransacked game tile into an island
        PurgeGameTile(tile, GameTileTypes.IslandTile);
    }

    public void TradeFood()
    {
        PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();

        obj.loot -= 1;
        obj.food +=5;
        // spawn some form of effects ?
        GenerateVillageTrade();
    }

    public void TradeWeapon()
    {
        PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();

        obj.loot -= 5;
        obj.weapons +=1;
        // spawn some form of effects ?
        GenerateTraderTrade();
    }

    // Strings for saga

    //fails 

     List<string> m_endGameEarlyStrings = new List<string>();

    void GenerateEndGameEarly()
    {
        PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();

        m_endGameEarlyStrings = new List<string>(){
        "And so "+obj.playerName+ " returned with a cerw of "+ obj.crew + " without having set sight on " + obj.vinland+".",
        "Finishing their journey early "+obj.playerName+ " and " + obj.personalPronoun + " crew could only hope to find "+ obj.vinland + " on their next voyage",
        ""+obj.playerName+ " received an urgent message from "+ obj.father+ " and "+obj.mother+". The search for " + obj.vinland + " would have to wait for another time."
        };

        sagaLogic.AddSagaContent(ReturnRandomString(m_endGameEarlyStrings));
    }

    List<string> m_endGameStarvationStrings = new List<string>();

    public void GenerateEndGameStarvation()
    {
        PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();

        m_endGameStarvationStrings = new List<string>(){
        "A good death is it's own reward, but "+obj.playerName+ " and "+ obj.personalPronoun +" crew would never know. Without food they perished one after the other until even "+obj.playerName+" had to submit to <color=red> Hel's<c/color> call.",
        ""+obj.playerName+ " and "+ obj.personalPronoun + " crew were able seaman and warriors, but even the hardiest Viking has to submit to hunger. They perished on their search for "+obj.vinland +" never to be seen again",
        "Oh norns, that you would have forseen the death of "+obj.playerName+obj.playerAttribute+ obj.father+ " and "+obj.mother+"by such a cruel thing as hunger. May <color=red> Hel<c/color> be mericiful on their souls."
        };

        sagaLogic.AddSagaContent(ReturnRandomString(m_endGameStarvationStrings));
    }

    List<string> m_endGameFailedRansackStrings = new List<string>();

    public void GenerateEndFailedRansack()
    {
        PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();

        m_endGameFailedRansackStrings = new List<string>(){
        "They say when you push a man or a woman too much over the edge, they will become bigger than life itself and, with a bit of help of <color=red>Tyr</color> or <color=red>Loki</color>even a pesant or simple trader will bring down a warrior.",
        "When "+obj.playerName+ " and "+ obj.personalPronoun +" crew tried to ransack the island, they were defeat to the last men and woman, by the desperate islanders defening their homes.",
        "Who would have thought that the norns foresaw this fate for "+obj.playerName+ "and "+ obj.personalPronoun +" crew? Everyone slay on the raid on an island. The messengers will have to cary sad new to Jarl"+ obj.father+ " and "+obj.mother+ " on this fateful day."
        };

        sagaLogic.AddSagaContent(ReturnRandomString(m_endGameFailedRansackStrings));
    }

    List<string> m_endGameFailedPirateStrings = new List<string>();

    public void GenerateEndFailedPirate()
    {
        PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();

        m_endGameFailedPirateStrings = new List<string>(){
        "And so our hero "+obj.playerName+" and all of the brave men and woman of "+obj.shipName+" were masacred by pirates."
        };

        sagaLogic.AddSagaContent(ReturnRandomString(m_endGameFailedPirateStrings));
    }

    List<string> m_endGameFailedSerpentStrings = new List<string>();

    public void GenerateEndFailedSerpent()
    {
        PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();

        m_endGameFailedSerpentStrings = new List<string>()
        {
        "With the shake of Jörmungandr's body "+obj.shipName+" was smashed, and even the best swimmers drowned in the wake of the its body's wave. <color=red>Rán</color> and her daughters readily took care of " +obj.playerName+ " and all "+obj.crew+ " of "+ obj.personalPronoun +" crew."
        };

        sagaLogic.AddSagaContent(ReturnRandomString(m_endGameFailedSerpentStrings));
    }

    // uneventful

    List<string> m_uneventfulJurneyStrings = new List<string>();
    public void GenerateUneventfulJourney()
    {
        PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();

        m_uneventfulJurneyStrings = new List<string>(){
        ""+obj.playerName+ " and "+ obj.personalPronoun +" crew travelled for several days. They passed a few islands some settled, others not.",
        "As the journey was uneventful for a couple of days, " +obj.playerName+ " and "+ obj.personalPronoun +" crew took good care of" + obj.shipName + "to make sure the vessle woul dbe ready when needed.",
        "The journey was uneventful and for a crew of "+obj.crew+" there was only so much game of dice before even those got boring. With well chosen words "+obj.playerName+" ralleyed the spirits of "+obj.playerAttribute +" warriors. Envigorated they continued."
        };

        sagaLogic.AddSagaContent(ReturnRandomString(m_uneventfulJurneyStrings));
    }
    // moving across islands
    List<string> m_crossIslandsStrings = new List<string>();
    public void GenerateCrossIslands()
    {
        PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();

        m_crossIslandsStrings = new List<string>(){
        "The shortest way was crossing the island. "+obj.playerName+ " and "+ obj.personalPronoun +" crew made landfall and used ropes and pulleys to get the ship across the island. Hard working warriors need sustenance and so they feasted after reaching the other side."
        };

        sagaLogic.AddSagaContent(ReturnRandomString(m_crossIslandsStrings));
    }


    // successfull

    List<string> m_raidedVillageStrings = new List<string>();
    public void GenerateRaidedVillage()
    {
        PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();

        m_raidedVillageStrings = new List<string>(){
        ""+obj.playerName+ " and "+ obj.personalPronoun +" crew ransacked a village and loaded the spoils into "+obj.shipName
        };

        sagaLogic.AddSagaContent(ReturnRandomString(m_raidedVillageStrings));
    }

    List<string> m_villageTradeStrings = new List<string>();
    public void GenerateVillageTrade()
    {
        PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();

        m_villageTradeStrings = new List<string>(){
        ""+obj.playerName+" traded provisions, meat and mead, in the village to continue the search for "+obj.vinland
        };

        sagaLogic.AddSagaContent(ReturnRandomString(m_villageTradeStrings));
    }

    List<string> m_raidedTraderStrings = new List<string>();
    public void GenerateRaidedTrader()
    {
        PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();

        m_raidedTraderStrings = new List<string>(){
        ""+obj.playerName+ " and "+ obj.personalPronoun +" crew ransacked the trading ost and left nothing whole."
        };

        sagaLogic.AddSagaContent(ReturnRandomString(m_raidedTraderStrings));
    }

    List<string> m_traderTradeStrings = new List<string>();
    public void GenerateTraderTrade()
    {
        PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();

        m_traderTradeStrings = new List<string>(){
        "Every warrior knows that a good weapon is worth it's weight in gold. So "+obj.playerName+" made sure that the journey to "+obj.vinland+ " would be well equipped."
        };

        sagaLogic.AddSagaContent(ReturnRandomString(m_traderTradeStrings));
    }

     List<string> m_beatPirateStrings = new List<string>();

    public void GenerateBeatPirate()
    {
        PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();

        m_beatPirateStrings = new List<string>(){
        "And so our hero "+obj.playerName+" and all of the brave men and woman of "+obj.shipName+" were masacred by pirates."
        };

        sagaLogic.AddSagaContent(ReturnRandomString(m_beatPirateStrings));
    }

    List<string> m_movedSerpentStrings = new List<string>();

    public void GenerateMovedSerpent()
    {
        PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();

        m_movedSerpentStrings = new List<string>()
        {
        "With courage "+obj.playerName+ " and "+ obj.personalPronoun +" warriors the curls of Jörmungandr's body. It moved the curl that blocked "+obj.shipName+" and dislodged by the thrown weapons, trease was found afloat, ready to be collected by "+obj.playerName+"'s crew."
        };

        sagaLogic.AddSagaContent(ReturnRandomString(m_movedSerpentStrings));
    }

    List<string> m_findingVinlandStrings = new List<string>();

    public void GenerateFoundVinland()
    {
        PlayerBehaviour obj = playerObject.GetComponent<PlayerBehaviour>();

        m_findingVinlandStrings = new List<string>()
        {
        "After a long journey "+obj.playerName+ " and "+ obj.personalPronoun +" warriors layed eyed on the place of myth and legend: "+obj.vinland+"! Long will the skald tell the tale of "+obj.playerName+" and "+ obj.personalPronoun +obj.shipName+ " and the men and woman that journey to the end of the know world."
        };

        sagaLogic.AddSagaContent(ReturnRandomString(m_findingVinlandStrings));
    }
}