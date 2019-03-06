using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectBehaviour : MonoBehaviour
{
    public enum InteractableObjectTypes{Island, Village, Trader, Pirate, MidgardSerpent, Secret}
    public InteractableObjectTypes interactableObject;
    public GameObject actionIndicator;
    public List<Mesh> actionIndicatorMesh = new List<Mesh>();

    bool lookedAt = false;
    // Start is called before the first frame update
    void Start()
    {
        DisableActionIndicator();
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        if(actionIndicator != null)
        {
            actionIndicator.transform.Rotate(Vector3.up, 5.0f*Time.deltaTime);
        }        
    }

    void InitialiseInteractableObject()
    {
        Mesh mesh = actionIndicator.GetComponent<MeshFilter>().mesh;
        switch(interactableObject)
        {
            case InteractableObjectTypes.Island :
                mesh = actionIndicatorMesh[0];
            break;

            case InteractableObjectTypes.Village :
                mesh = actionIndicatorMesh[1];
            break;

            case InteractableObjectTypes.Trader :
                mesh = actionIndicatorMesh[2];
            break;

            case InteractableObjectTypes.Pirate :
                mesh = actionIndicatorMesh[3];
            break;

            case InteractableObjectTypes.MidgardSerpent :
                mesh = actionIndicatorMesh[4];
            break;

            case InteractableObjectTypes.Secret :
                mesh = actionIndicatorMesh[5];
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
}