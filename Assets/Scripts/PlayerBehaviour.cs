using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    RaycastHit hit;
    public float rotationTime = 0.0f;

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
}
