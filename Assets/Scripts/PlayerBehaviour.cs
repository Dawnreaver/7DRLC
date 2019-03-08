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
        "Bjørn","Brynjar","Dag","Einar","Erik","Gunnar","Helgi","Hlynur","Ingmar","Ivar","Kjartan","Knut","Leif","Njal","Olaf",
        "Örn","Ragnar","Randi","Sigur","Sindri","Svend","Tor"
    };
    private List<string> m_maleAttributes = new List<string>()
    {
        "the great","the strong","the wise","the red","the grey","the meek","the mighty","the brave","the lucky","the trickster",
        "the thick","the merry","the bear","the sage","the wolf","the dragon slayer","killer of giants", ""
    };
    private List<string> m_femaleNames = new List<string>()
    {
        "Aðalheiður","Astrid","Björg","Elin","Esther","Freya","Frigg","Gildur","Guðrún","Gunhild","Hjördís","Ingrid","Minna","Mista",
        "Ragnhildur","Saga","Salla","Sigrid","Siv","Solveig","Thelma","Tuulia","Vigdís"
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
    private GameObject m_lastLookedAtObject;
    // movement
    public float rotationTime = 0.0f;
    public bool pathisBlocked = false;

    // inventory
    public int crew = 0;
    public int food = 0;
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
                    PlayerConsumeFood(1);
                    Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z+moveDistance);
                    transform.position = newPosition;
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
                    PlayerConsumeFood(1);
                    Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z-moveDistance);
                    transform.position = newPosition;
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
                    PlayerConsumeFood(1);
                    Vector3 newPosition = new Vector3(transform.position.x-moveDistance, transform.position.y, transform.position.z);
                    transform.position = newPosition;
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
                    PlayerConsumeFood(1);
                    Vector3 newPosition = new Vector3(transform.position.x+moveDistance, transform.position.y, transform.position.z);
                    transform.position = newPosition;
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

            if(m_lastLookedAtObject == null)
            {
                m_lastLookedAtObject = hit.collider.gameObject;
            }
            else if(hit.collider.gameObject != m_lastLookedAtObject)
            {
                m_lastLookedAtObject.GetComponent<GameTileBehaviour>().DisableActionIndicator();
                m_lastLookedAtObject = hit.collider.gameObject;
            }
            // impassable tiles: Start Village, village(has to be raised and turned into an island), trader, secret, vinland
            if(obj.tileType == GameTileTypes.StartVillageTile || obj.tileType == GameTileTypes.VillageTile
            ||obj.tileType == GameTileTypes.VinlandTile || obj.tileType == GameTileTypes.SecretTile || obj.tileType == GameTileTypes.TraderTile)
            {
                pathisBlocked = true;
            }
            else
            {
                pathisBlocked = false;
            }

            if(obj.tileType != GameTileTypes.WaterTile)
            {
                obj.EnableActionIndicator();
            }
            /* if(obj.tileType == GameTileTypes.SecretTile || 
                obj.tileType ==GameTileTypes.VillageTile || 
                obj.tileType == GameTileTypes.StartVillageTile ||
                obj.tileType == GameTileTypes.StartVillageTile)
            {
                pathisBlocked = true;
            }
            else
            {

            }
            obj.EnableActionIndicator();
            Debug.Log(hit.collider.gameObject.tag);*/
        }
        /*else
        {
            if(m_lastLookedAtObject != null)
            {
                m_lastLookedAtObject.GetComponent<GameTileBehaviour>().DisableActionIndicator();
            }
            pathisBlocked = false;
        }*/
    }

    void OnGizmoDraw()
    {
        //Gizmos.color = Color.red;
        Vector3 pos = transform.position+transform.forward;
        Gizmos.DrawSphere(pos, 1.0f);
    }

    public void InitialisePlayer()
    {
        food = 24;
        crew = Random.Range(36,41);
        loot = 5;
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
                playerName = "<color=red>"+ReturnRandomString(m_femaleNames)+"</color>";
                playerAttribute = ", daughter of Jarl ";
                personalPronoun = " her ";
            break;

            case PlayerGenders.Male : 
                playerName = "<color=red>"+ReturnRandomString(m_maleNames)+"</color>";
                playerAttribute = ", son of Jarl ";
                personalPronoun = " his ";
            break;
        }

        shipName = "<color=red>"+ReturnRandomString(m_shipNames)+"</color>";
        father = "<color=red>"+ ReturnRandomString(m_maleNames, playerName)+" "+ReturnRandomString(m_maleAttributes)+"</color>";
        mother = "<color=red>"+ ReturnRandomString(m_femaleNames,playerName)+" "+ReturnRandomString(m_femaleAttributes)+"</color>";
        month = ReturnRandomString(m_monthsOfTheVikingYear);
        vinland = "<color=red>Vinland</color>";
       
       gameLogic.sagaLogic.AddSagaContent( "This is the story of "+playerName + playerAttribute + father+" and " + mother + ", who set out in the month of " + month + " on" + personalPronoun + "proud vessel " + shipName + " with a band of " + crew + " on the search for " + vinland + ".");
    }

    string ReturnRandomString(List<string> strings, string unique = null)
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

    string ReturnRandomString(string[] strings, string unique = null)
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
}
