using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraBehaviour : MonoBehaviour
{
    public GameObject followObject;

    public float cameraFollowOffsetX = 0.0f, cameraFollowOffsetY = 3.0f, cameraFollowOffsetZ = -1.0f;
    private float m_movementSpeed = 5.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(followObject)
        {
            MoveMainCameraWithFollowObject(followObject.transform.position);
        }
        else
        {
            Debug.LogError("MainCameraBehaviour: No object to follow.");
        }
    }
    void  MoveMainCameraWithFollowObject(Vector3 followObjectPosition)
    {
        Vector3 newPosition = new Vector3(followObjectPosition.x+cameraFollowOffsetX,followObjectPosition.y+cameraFollowOffsetY,followObjectPosition.z+cameraFollowOffsetZ);
        transform.position = Vector3.MoveTowards(transform.position, newPosition,m_movementSpeed*Time.deltaTime);
    }

    public void SetStartPosition()
    {
        Vector3 newPosition = new Vector3(followObject.transform.position.x+cameraFollowOffsetX,followObject.transform.position.y+cameraFollowOffsetY,followObject.transform.position.z+cameraFollowOffsetZ);
        transform.position = newPosition;
    }

    public void SetCameraMovementSpeed(float speed)
    {
        m_movementSpeed = speed;
    }
}
