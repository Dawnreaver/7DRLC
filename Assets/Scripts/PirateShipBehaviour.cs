using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateShipBehaviour : MonoBehaviour
{
    public bool debug = false;
    public GameLogic gameLogic;
    public GameObject preyObject;
    public Vector2 tileOrigin = new Vector2();
    private int m_lineOfSight =4;
    public bool sightedPrey = false;
    private float m_distanceToPrey;
    private int m_ketchUpEstimate = 0;
    public int followThreshold = 15; // number of fields the pirate will follow beforr abandoning the chase
    private int m_follow = 0;
    public bool takeTurn = false;
    public List<Vector3> m_pathToPrey = new List<Vector3>();

    public bool foundPathToPrey = false;

    private Vector3 m_oldPreyPosition;

    void FixedUpdate(){

        if(takeTurn){

            EnemyTakeTurn();
            takeTurn = false;
        }
    }

    public void EnemyTakeTurn(){

        if(!sightedPrey){

            TurnInRandomDirection();
            SpottingPrey();   
        }
        else{

            FollowPreyObject();
        }
    }

    void SpottingPrey(){

        Vector2 pPos = new Vector2(preyObject.transform.position.x,preyObject.transform.position.z);
        Vector2 sPos = new Vector2();
        float viewDirection = transform.localRotation.y;

        switch(viewDirection){
            // up
            case 0.0f :
                for(int a = 0; a < m_lineOfSight; a++){

                    sPos = new Vector2(transform.position.x,transform.position.z+a);
                    
                    if(pPos == sPos){

                        sightedPrey = true;
                    }
                }
            break;
            // down
            case 1.0f :
                for(int b = 0; b < m_lineOfSight; b++){

                    sPos = new Vector2(transform.position.x,transform.position.z-b);
                    
                    if(pPos == sPos){
                        
                        sightedPrey = true;
                    }
                }
            break;
            //left
            case -0.7071068f :
                for(int c = 0; c < m_lineOfSight; c++){

                    sPos = new Vector2(transform.position.x-c,transform.position.z);
                    
                    if(pPos == sPos){

                        sightedPrey = true;
                    }
                }
            break;
            // right
            case 0.7071068f :
                for(int d = 0; d < m_lineOfSight; d++){

                    sPos = new Vector2(transform.position.x+d,transform.position.z);
                    
                    if(pPos == sPos){

                        sightedPrey = true;
                    }
                }
            break;
        }
        if(sightedPrey){
             FindPathToPrey();
        }
    }

    void FollowPreyObject(){
        /*if(m_oldPreyPosition != preyObject.transform.position)
        {
            Debug.Log("Player has new position.");
             foundPathToPrey = false;
             m_oldPreyPosition = preyObject.transform.position;
        } 

        if(!foundPathToPrey)
        {
            FindPathToPrey();
            transform.position = m_pathToPrey[m_pathToPrey.Count-1];
            m_pathToPrey.Remove(m_pathToPrey[m_pathToPrey.Count-1]);
            Debug.Log("Follow: No path first");
        }
        else
        {
            transform.position = m_pathToPrey[m_pathToPrey.Count-1];
            m_pathToPrey.Remove(m_pathToPrey[m_pathToPrey.Count-1]);
            Debug.Log("Follow: Path first");

            if(m_pathToPrey.Count == 0)
            {
                Debug.Log("Fighting player");
            }
        }*/

        FindPathToPrey();
        transform.position = m_pathToPrey[m_pathToPrey.Count-1];
        m_pathToPrey.Remove(m_pathToPrey[m_pathToPrey.Count-1]);

    }

    void TurnInRandomDirection(){

        int randomShipRotation = Random.Range(0,4);

        if(randomShipRotation == 0){

            transform.rotation = Quaternion.LookRotation(Vector3.forward,Vector3.up);
        }
        else if(randomShipRotation == 1){

            transform.rotation = Quaternion.LookRotation(Vector3.right,Vector3.up);
        }
        else if(randomShipRotation == 2){

            transform.rotation = Quaternion.LookRotation(Vector3.back,Vector3.up);
        }
        else{

            transform.rotation = Quaternion.LookRotation(Vector3.left,Vector3.up);
        }
    }

    void ResetPirateShip(){

        m_follow = 0;
    }

    void FindPathToPrey(){

        m_pathToPrey.Clear();

        m_pathToPrey.Add(preyObject.transform.position);

        for (int a = 0; a < 5; a++){

            Vector3 currentPos = m_pathToPrey[m_pathToPrey.Count-1];
            Vector3 newPos = TryNextNodeCloserToMe(currentPos);

            if(newPos.x == transform.position.x && newPos.z == transform.position.z){

                 foundPathToPrey =  true;
                break;
            } 

            else if(newPos.x != transform.position.x || newPos.z != transform.position.z){

                m_pathToPrey.Add(newPos);
            }
        }       
    }
    Vector3 TryNextNodeCloserToMe(Vector3 currentPos){

        Vector3 newPos = new Vector3();

        float   distanceToMyself = 200.0f;

        List<Vector3> possiblePositions = new List<Vector3>(){

            new Vector3(currentPos.x, currentPos.y, currentPos.z+1),
            new Vector3(currentPos.x, currentPos.y, currentPos.z-1),
            new Vector3(currentPos.x+1, currentPos.y, currentPos.z),
            new Vector3(currentPos.x-1, currentPos.y, currentPos.z)
        };

        for(int a = 0; a < possiblePositions.Count;a++){

            for(int b = 0; b < gameLogic.usedGameTiles.Count; b++){

                if(gameLogic.usedGameTiles[b].transform.position.x == possiblePositions[a].x && 
                gameLogic.usedGameTiles[b].transform.position.z == possiblePositions[a].z  && 
                gameLogic.usedGameTiles[b].GetComponent<GameTileBehaviour>().tileType == GameTileTypes.WaterTile ||
                gameLogic.usedGameTiles[b].transform.position.x == possiblePositions[a].x && 
                gameLogic.usedGameTiles[b].transform.position.z == possiblePositions[a].z  && 
                gameLogic.usedGameTiles[b].GetComponent<GameTileBehaviour>().tileType == GameTileTypes.PirateTile){

                    for(int c = 0; c < gameLogic.m_activePirateShips.Count; c++){

                        if( gameLogic.m_activePirateShips[c].transform.position.x != possiblePositions[a].x && 
                            gameLogic.m_activePirateShips[c].transform.position.z != possiblePositions[a].z && 
                            gameLogic.m_activePirateShips[c] != gameObject){

                            float tempDistance = Vector3.Distance(possiblePositions[a], gameObject.transform.position);

                            if( tempDistance < distanceToMyself){

                                distanceToMyself = tempDistance;
                                newPos = possiblePositions[a];
                            }
                        }
                    }
                }
            }
        }
        return newPos;
    }
        // for(int a = 0; a < possiblePositions.Count;a++)
        // {
        //     for(int b = 0; b < gameLogic.usedGameTiles.Count; b++)
        //     {
        //         if(gameLogic.usedGameTiles[b].transform.position.x == possiblePositions[a].x && gameLogic.usedGameTiles[b].transform.position.z == possiblePositions[a].z  && gameLogic.usedGameTiles[b].GetComponent<GameTileBehaviour>().tileType == GameTileTypes.WaterTile)
        //         {
                   
        //             float tempDistance = Vector3.Distance(possiblePositions[a], transform.position);

        //             for(int c = 0; c < gameLogic.m_activePirateShips.Count; c++)
        //             {
        //                 if(gameLogic.m_activePirateShips[c].transform.position.x != possiblePositions[a].x && gameLogic.m_activePirateShips[c].transform.position.z != possiblePositions[a].z)
        //                 {
        //                     if( tempDistance < distanceToMyself)
        //                     {
        //                         //Debug.Log("Found position: "+ possiblePositions[a] + " Distance: "+ tempDistance);
        //                         distanceToMyself = tempDistance;
        //                         newPos = possiblePositions[a];
        //                     }
        //                 }
        //             }
        //         }
        //     }
        // }
    void OnDrawGizmos(){

        for(int a = 0; a < m_pathToPrey.Count; a++){
            
            Vector3 pos = new Vector3(m_pathToPrey[a].x, 1.0f, m_pathToPrey[a].z);
            Gizmos.DrawSphere(pos,0.25f);
        }
        
    }
}
