using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateShipBehaviour : MonoBehaviour
{
    public bool debug = false;

    public GameLogic gameLogic;
    public GameObject preyObject;
    public Vector2 tileOrigin = new Vector2();
    public int lineOfSight =2;
    public bool sightedPrey = false;
    private float m_distanceToPrey;
    private int m_ketchUpEstimate = 0;
    public int followThreshold = 15; // number of fields the pirate will follow beforr abandoning the chase
    private int m_follow = 0;
    public bool takeTurn = false;

    void FixedUpdate()
    {
        if(debug)
        {
            Debug.Log(transform.localRotation.y);
        }
        if(takeTurn)
        {
            EnemyTakeTurn();
            takeTurn = false;
        }
    }

    public void EnemyTakeTurn()
    {
        if(!sightedPrey)
        {
            SpottingPrey();
            //TurnInRandomDirection();
        }
        else
        {
            FollowPreyObject();
        }
    }

    void SpottingPrey()
    {
        Vector2 pPos = new Vector2(preyObject.transform.position.x,preyObject.transform.position.z);
        Vector2 sPos = new Vector2();
        float viewDirection = transform.localRotation.y;

        switch(viewDirection)
        {
            // up
            case 0.0f :
                for(int a = 0; a < lineOfSight; a++)
                {
                    sPos = new Vector2(transform.position.x,transform.position.z+a);
                    
                    if(pPos == sPos)
                    {
                        Debug.Log("Spotted preyObject");
                        sightedPrey = true;
                    }
                }
            break;
            // down
            case 1.0f :
                for(int b = 0; b < lineOfSight; b++)
                {
                    sPos = new Vector2(transform.position.x,transform.position.z-b);
                    
                    if(pPos == sPos)
                    {
                        Debug.Log("Spotted preyObject");
                        sightedPrey = true;
                    }
                }
            break;
            //left
            case -0.7071068f :
                for(int c = 0; c < lineOfSight; c++)
                {
                    sPos = new Vector2(transform.position.x-c,transform.position.z);
                    
                    if(pPos == sPos)
                    {
                        Debug.Log("Spotted preyObject");
                        sightedPrey = true;
                    }
                }
            break;
            // right
            case 0.7071068f :
                for(int d = 0; d < lineOfSight; d++)
                {
                    sPos = new Vector2(transform.position.x+d,transform.position.z);
                    
                    if(pPos == sPos)
                    {
                        Debug.Log("Spotted preyObject");
                        sightedPrey = true;
                    }
                }
            break;
        }
    }

    void FollowPreyObject()
    {
        Vector3 newPosition;
        float distanceToPlayer = 2000.0f;

        List<Vector3> possiblePositions = new List<Vector3>()
        {
            new Vector3(transform.position.x, gameLogic.gameTileStartPositionHeight, transform.position.z+1),
            new Vector3(transform.position.x, gameLogic.gameTileStartPositionHeight, transform.position.z-1),
            new Vector3(transform.position.x-1, gameLogic.gameTileStartPositionHeight, transform.position.z),
            new Vector3(transform.position.x+1, gameLogic.gameTileStartPositionHeight, transform.position.z)
        };
        Debug.Log("Looking for position close to player...");
        for(int a = 0; a < possiblePositions.Count;a++)
        {
            for(int b = 0; b < gameLogic.usedGameTiles.Count; b++)
            {
                if(gameLogic.usedGameTiles[b].transform.position == possiblePositions[a] && gameLogic.usedGameTiles[b].GetComponent<GameTileBehaviour>().tileType == GameTileTypes.WaterTile)
                {
                   
                    float tempDistance = Vector3.Distance(possiblePositions[a], preyObject.transform.position);

                    if( tempDistance < distanceToPlayer)
                    {
                         Debug.Log("Found position: "+ possiblePositions[a]);
                        distanceToPlayer = tempDistance;
                        newPosition = new Vector3(possiblePositions[a].x, transform.position.y,possiblePositions[a].z);
                        Debug.Log("Move to position...");
                        transform.position = newPosition;
                    }
                }
            }
        }


    }

    void TurnInRandomDirection()
    {
        
    }

    void ResetPirateShip()
    {
        m_follow = 0;
    }
}
