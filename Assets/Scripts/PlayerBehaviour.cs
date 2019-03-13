using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public GameLogic gameLogic;
    // randomization
    public string playerName, father, mother, month, playerAttribute, shipName,vinland, personalPronoun;
    public enum PlayerGenders{Female, Male}
    public PlayerGenders playerGender;
    private List<string> m_maleNames = new List<string>()
    {
        /*"Bjørn","Brynjar","Dag","Einar","Erik","Gunnar","Helgi","Hlynur","Ingmar","Ivar","Kjartan","Knut","Leif","Njal","Olaf",
        "Örn","Ragnar","Randi","Sigur","Sindri","Svend","Tor"*/
        "Arne",
        "Birger",
        "Bjørn",
        "Bo",
        "Erik",
        "Frode",
        "Gorm",
        "Halfdan",
        "Harald", 
        "Knud",
        "Kåre",
        "Leif",
        "Njal",
        "Roar",
        "Rune",
        "Sten",
        "Skarde",
        "Sune",
        "Svend",
        "Troels",
        "Toke",
        "Torsten",
        "Trygve",
        "Ulf",
        "Ødger",
        "Åge"
    };
    private List<string> m_maleAttributes = new List<string>()
    {
        "the great","the strong","the wise","the red","the grey","the meek","the mighty","the brave","the lucky","the trickster",
        "the thick","the merry","the bear","the sage","the wolf","the dragon slayer","killer of giants", ""
    };
    private List<string> m_femaleNames = new List<string>()
    {
        /*"Aðalheiður","Astrid","Björg","Elin","Esther","Freya","Frigg","Gildur","Guðrún","Gunhild","Hjördís","Ingrid","Minna","Mista",
        "Ragnhildur","Saga","Salla","Sigrid","Siv","Solveig","Thelma","Tuulia","Vigdís"*/

        "Astrid",
        "Bodil",
        "Frida",
        "Gertrud",
        "Gro",
        "Estrid",
        "Hilda",
        "Gudrun",
        "Gunhild",
        "Helga",
        "Inga",
        "Liv",
        "Rndi",
        "Signe",
        "Sigrid",
        "Revna",
        "Sif",
        "Tora",
        "Tove",
        "Thyra",
        "Thurid",
        "Yrsa",
        "Ulfhild",
        "Åse"
    };
    private List<string> m_femaleAttributes = new List<string>()
    {
        "the beautiful","silver toungue","the bountyful","the quick","the quiet","the wicked","the allseing","moon child","the frostbarer",
        "the fearles","the whitty","the swan","the sensible","the enchantress","the shieldmaid","slayer of sylphs","the unyielding", ""
    };
    private List<string> m_shipNames = new List<string>()
    {
        "Alptr","Brimdýr","Dreki","Fälki","Fífa","Görn","Grágás","Gullbringa","Hestr svanfjalla","Hringhorni","Járnmeiss","Mariusuð","Naglfar",
        "Skíðblaðnir","Skjöldr","Svalbarði","Tráni","Trékyllir","Unnblakkr","Uxi","Vísundr","Yðliða"
    };

    string[] m_monthsOfTheVikingYear = new string[]
    {
        //winter months
        "Gormánuður",
        "Ýlir",
        "Mörsugur",
        "Þorri",
        "Gói",
        "Einmánuður",
        // summer months
        "Harpa",
        "Skerpla",
        "Sólmánuður",
        "Heyannir",
        "Tvímánuður",
        "Haustmánuður"
    };

    // recognise environment
    RaycastHit hit;
    public GameObject lastLookedAtObject;
    // movement
    public float rotationTime = 0.0f;
    public bool pathisBlocked = false;

    // inventory
    public int crew = 0;
    public int startFood = 0;
    public int food = 0;
    public int startLoot = 0;
    public int loot = 0;
    public int weapons = 0;
    
    void Start()
    {
        /*for( int a = 0; a < 10; a++)
        {
            RandomisePlayer();
        }*/
    }

    void Update()
    {
        GameTileBehaviour obj = lastLookedAtObject.GetComponent<GameTileBehaviour>();
        if(gameLogic.gameStarted && !gameLogic.menuEnabled)
        {
            float moveDistance = 1.0f;
            if(Input.GetKeyDown("w") || Input.GetKeyDown("up"))
            {
                if(transform.localRotation.y != 0.0f)
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.forward,Vector3.up);
                }
                else if (transform.localRotation.y == 0.0f && !pathisBlocked)
                {
                    if(obj.tileType == GameTileTypes.IslandTile)
                    {
                        PlayerConsumeFood(2);
                        gameLogic.GenerateCrossIslands();
                    }
                    else
                    {
                        PlayerConsumeFood(1);
                    }
                    Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z+moveDistance);
                    transform.position = newPosition;
                    CheckForUneventulJourney();
                    CheckForAttackOnGameTile();
                    CheckIfReachedVinland();
                }
            }
            if(Input.GetKeyDown("s")|| Input.GetKeyDown("down"))
            {
                if(transform.localRotation.y != 1.0f)
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.back,Vector3.up);
                }
                else if (transform.localRotation.y == 1.0f && !pathisBlocked)
                {
                    if(obj.tileType == GameTileTypes.IslandTile && obj.isRansacked == 0|| obj.tileType == GameTileTypes.VillageTile && obj.isRansacked == 1 || obj.tileType == GameTileTypes.TraderTile && obj.isRansacked == 1)
                    {
                        PlayerConsumeFood(2);
                        gameLogic.GenerateCrossIslands();
                    }
                    else
                    {
                        PlayerConsumeFood(1);
                    }
                    Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z-moveDistance);
                    transform.position = newPosition;
                    CheckForUneventulJourney();
                    CheckForAttackOnGameTile();
                    CheckIfReachedVinland();
                }
            }
            if(Input.GetKeyDown("a")|| Input.GetKeyDown("left"))
            {
                if(transform.localRotation.y != -0.7071068f)
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.left,Vector3.up);
                }
                else if (transform.localRotation.y == -0.7071068f && !pathisBlocked)
                {
                    if(obj.tileType == GameTileTypes.IslandTile)
                    {
                        PlayerConsumeFood(2);
                        gameLogic.GenerateCrossIslands();
                    }
                    else
                    {
                        PlayerConsumeFood(1);
                    }
                    Vector3 newPosition = new Vector3(transform.position.x-moveDistance, transform.position.y, transform.position.z);
                    transform.position = newPosition;
                    CheckForUneventulJourney();
                    CheckForAttackOnGameTile();
                    CheckIfReachedVinland();
                }
            }
            if(Input.GetKeyDown("d")|| Input.GetKeyDown("right"))
            {
                if(transform.localRotation.y != 0.7071068f)
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.right,Vector3.up);
                }
                else if (transform.localRotation.y == 0.7071068f && !pathisBlocked)
                {
                    if(obj.tileType == GameTileTypes.IslandTile)
                    {
                        PlayerConsumeFood(2);
                        gameLogic.GenerateCrossIslands();
                    }
                    else
                    {
                        PlayerConsumeFood(1);
                    }
                    Vector3 newPosition = new Vector3(transform.position.x+moveDistance, transform.position.y, transform.position.z);
                    transform.position = newPosition;
                    CheckForUneventulJourney();
                    CheckForAttackOnGameTile();
                    CheckIfReachedVinland();
                }
            }
        }
    }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward,Color.red);
        if(Physics.Raycast(transform.position,transform.forward, out hit, 1.0f))
        {
            GameTileBehaviour obj = hit.collider.gameObject.GetComponent<GameTileBehaviour>();

            if(lastLookedAtObject == null)
            {
                lastLookedAtObject = hit.collider.gameObject;
            }
            else if(hit.collider.gameObject != lastLookedAtObject)
            {
                lastLookedAtObject.GetComponent<GameTileBehaviour>().DisableActionIndicator();
                lastLookedAtObject = hit.collider.gameObject;
            }
            // impassable tiles: Start Village, village(has to be raised and turned into an island), trader, secret, vinland
            if(obj.tileType == GameTileTypes.StartVillageTile && obj.isRansacked == 0|| obj.tileType == GameTileTypes.VillageTile  && obj.isRansacked == 0
             || obj.tileType == GameTileTypes.TraderTile  && obj.isRansacked == 0) //|| obj.tileType == GameTileTypes.SecretTile ||obj.tileType == GameTileTypes.VinlandTile 
            {
                pathisBlocked = true;
            }
            else
            {
                pathisBlocked = false;
            }

            if(obj.tileType != GameTileTypes.WaterTile || obj.tileType != GameTileTypes.PirateTile || obj.tileType != GameTileTypes.SerpentTile)
            {
                if(transform.localRotation.y == 0.0f)
                {
                    obj.EnableActionIndicator(Vector3.forward); 
                }
                else if(transform.localRotation.y == 0.7071068f)
                {
                    obj.EnableActionIndicator(Vector3.right);  
                }
                else if(transform.localRotation.y == 1.0f)
                {
                    obj.EnableActionIndicator(Vector3.back);
                }
                else
                {
                    obj.EnableActionIndicator(Vector3.left);
                } 
            }
        }
    }

    void CheckForUneventulJourney()
    {
        GameTileBehaviour obj = lastLookedAtObject.GetComponent<GameTileBehaviour>();

        if(obj.tileType == GameTileTypes.WaterTile)
        {
            gameLogic.uneventfulJourneyCount +=1;
                if(gameLogic.uneventfulJourneyCount >= 5)
            {
                gameLogic.GenerateUneventfulJourney();
                gameLogic.uneventfulJourneyCount = 0;
            }
        }
        else
        {
            gameLogic.uneventfulJourneyCount = 0;
        }
    }

    public void CheckForAttackOnGameTile()
    {
        for(int a = 0; a < gameLogic.usedGameTiles.Count; a++)
        {
            GameTileBehaviour tile = gameLogic.usedGameTiles[a].GetComponent<GameTileBehaviour>();

            if(gameLogic.usedGameTiles[a].transform.position.x == transform.position.x && gameLogic.usedGameTiles[a].transform.position.z == transform.position.z)
            {
                if(tile.tileType == GameTileTypes.SerpentTile || tile.tileType == GameTileTypes.PirateTile)
                {
                    Debug.Log("Attacking: "+tile.tileType.ToString());
                    gameLogic.AttackGameTile(gameLogic.usedGameTiles[a]);
                }
            }
        }
    }

    public void CheckIfReachedVinland()
    {
        for(int b = 0; b < gameLogic.usedGameTiles.Count; b++)
        {
            GameTileBehaviour tile = gameLogic.usedGameTiles[b].GetComponent<GameTileBehaviour>();

            if(gameLogic.usedGameTiles[b].transform.position.x == transform.position.x && gameLogic.usedGameTiles[b].transform.position.z == transform.position.z)
            {
                if(tile.tileType == GameTileTypes.VinlandTile)
                {
                    gameLogic.WinGame();
                }
            }
        }
    }


    public void InitialisePlayer()
    {
        food = startFood;
        crew = Random.Range(36,41);
        loot = startLoot;
        weapons = 0;
        RandomisePlayer();
    }

    public void PlayerConsumeFood(int amount)
    {
        if( food >= amount && food > 0)
        {
            food -= amount;
        }
        else if( food < amount && food > 0)
        {
            food = 0;
        }
        else
        {
            crew -= Random.Range(2,6);

            if(crew <= 0)
            {
                crew = 0;

                gameLogic.GenerateEndGameStarvation();
                Debug.Log("Lost Game: Starvation");
            }
        }
    }

    public void RandomisePlayer()
    {
        playerGender = (PlayerGenders) Random.Range(0,2);

        switch(playerGender)
        {
            case PlayerGenders.Female :
                playerName = "<color=red>"+gameLogic.ReturnRandomString(m_femaleNames)+"</color>";
                playerAttribute = ", daughter of Jarl ";
                personalPronoun = " her ";
            break;

            case PlayerGenders.Male : 
                playerName = "<color=red>"+gameLogic.ReturnRandomString(m_maleNames)+"</color>";
                playerAttribute = ", son of Jarl ";
                personalPronoun = " his ";
            break;
        }

        shipName = "<color=red>"+gameLogic.ReturnRandomString(m_shipNames)+"</color>";
        father = "<color=red>"+ gameLogic.ReturnRandomString(m_maleNames, playerName)+" "+gameLogic.ReturnRandomString(m_maleAttributes)+"</color>";
        mother = "<color=red>"+ gameLogic.ReturnRandomString(m_femaleNames, playerName)+" "+gameLogic.ReturnRandomString(m_femaleAttributes)+"</color>";
        month = gameLogic.ReturnRandomString(m_monthsOfTheVikingYear);
        vinland = "<color=red>Vinland</color>";
       
       gameLogic.sagaLogic.AddSagaContent( "This is the story of "+playerName + playerAttribute + father+" and " + mother + ", who set out in the month of " + month + " on" + personalPronoun + "proud vessel " + shipName + " with a band of " + crew + " on the search for " + vinland + ".");
    }
}
