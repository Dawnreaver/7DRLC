using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public SOPlayer player = new SOPlayer();
    public string plalayerName;

    public string playerAttribute = "";
    public string shipName;

    public enum PlayerGenders{Female, Male}
    public PlayerGenders playerGender;
    private List<string> m_maleNames = new List<string>()
    {
        "Bjørn",
        "Brynjar",
        "Dag",
        "Einar",
        "Erik",
        "Gunnar",
        "Helgi",
        "Hlynur",
        "Ingmar",
        "Ivar",
        "Kjartan",
        "Knut",
        "Leif",
        "Njal",
        "Olaf",
        "Örn",
        "Ragnar",
        "Randi",
        "Sigur",
        "Sindri",
        "Svend",
        "Tor"
    };
    private List<string> m_maleAttributes = new List<string>()
    {
        "the great",
        "the strong",
        "the wise",
        "the red",
        "the grey",
        "the meek",
        "the mighty",
        "the brave",
        "the lucky",
        "the trickster",
        "the thick",
        "the merry",
        "the bear",
        "the sage",
        "the wolf",
        "the dragon slayer",
        "killer of giants"
    };
    private List<string> m_femaleNames = new List<string>()
    {
        "Aðalheiður",
        "Astrid",
        "Björg",
        "Elin",
        "Esther",
        "Freya",
        "Frigg",
        "Gildur",
        "Guðrún",
        "Gunhild",
        "Hjördís",
        "Ingrid",
        "Minna",
        "Mista",
        "Ragnhildur",
        "Saga",
        "Salla",
        "Sigrid",
        "Siv",
        "Solveig",
        "Thelma",
        "Tuulia",
        "Vigdís"
    };
    private List<string> m_femaleAttributes = new List<string>()
    {
        "the beautiful",
        "silver toungue",
        "the bountyful",
        "the quick",
        "the quiet",
        "the wicked",
        "the allseing",
        "moon child",
        "the frostbarer",
        "the fearles",
        "the whitty",
        "the swan",
        "the sensible",
        "the enchantress",
        "the shieldmaid",
        "slayer of sylphs",
        "the unyielding"
    };
    private List<string> m_shipNames = new List<string>()
    {
        "Alptr",
        "Brimdýr",
        "Dreki",
        "Fälki",
        "Fífa",
        "Görn",
        "Grágás",
        "Gullbringa",
        "Hestr svanfjalla",
        "Hringhorni",
        "Járnmeiss",
        "Mariusuð",
        "Naglfar",
        "Skíðblaðnir",
        "Skjöldr",
        "Svalbarði",
        "Tráni",
        "Trékyllir",
        "Unnblakkr",
        "Uxi",
        "Vísundr",
        "Yðliða"
    };

    RaycastHit hit;
    public float rotationTime = 0.0f;

    void Start()
    {
        for(int a = 0; a < 10; a++)
        {
            RandomisePlayer();
        }
    }

    void Update()
    {
        float moveDistance = 1.0f;
        if(Input.GetKeyDown("w"))
        {
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z+moveDistance);
            transform.position = newPosition;
            StartCoroutine("RotateMe",0.0f);
        }
         if(Input.GetKeyDown("s"))
        {
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z-moveDistance);
            transform.position = newPosition;
            StartCoroutine("RotateMe",180.0f);
        }
         if(Input.GetKeyDown("a"))
        {
            Vector3 newPosition = new Vector3(transform.position.x-moveDistance, transform.position.y, transform.position.z);
            transform.position = newPosition;
           StartCoroutine("RotateMe",270.0f);
        }
         if(Input.GetKeyDown("d"))
        {
            Vector3 newPosition = new Vector3(transform.position.x+moveDistance, transform.position.y, transform.position.z);
            transform.position = newPosition;
            StartCoroutine("RotateMe",90.0f);
        }

       /* if(Input.GetButtonDown("Fire1"))
        {
            RotateLeft();
        }*/
    }

    void FixedUpdate()
    {
        Debug.DrawLine(transform.position, transform.position+transform.forward,Color.red);

        if(Physics.Raycast(transform.position,transform.position+transform.forward, out hit, 1.0f))
        {
            Debug.Log(hit.collider.tag);
        }

    }

    IEnumerator RotateMe(float newRotation)
    {
        //Quaternion myNewRotation = Quaternion.AngleAxis(newRotation, Vector3.up);
        //transform.rotation = Quaternion.Slerp(transform.rotation, myNewRotation, rotationTime);
        transform.Rotate(0.0f, newRotation, 0.0f);
        return null;

    }

    public void RandomisePlayer()
    {
        int randomNumber;

        playerGender = (PlayerGenders) Random.Range(0,2);

        switch(playerGender)
        {
            case PlayerGenders.Female :
                plalayerName = ReturnRandomString(m_femaleNames);
                playerAttribute = ", daughter of Jarl ";
            break;

            case PlayerGenders.Male : 
                plalayerName = ReturnRandomString(m_femaleNames);
                playerAttribute = ", son of Jarl ";
            break;
        }

        shipName = ReturnRandomString(m_shipNames);

        Debug.Log("This is the story of "+plalayerName+playerAttribute+ReturnRandomString(m_maleNames,plalayerName)+" who set out on the proud vessel "+shipName+" with a band of "+Random.Range(48,69)+" on the search for Vinland.");

    }

    string ReturnRandomString(List<string> strings)
    {
        string randomString;

        randomString = strings[Random.Range(0,lists.Count-1)];

        return randomString;
    }

    string ReturnRandomString(List<string> strings, string exception)
    {

    }
}
