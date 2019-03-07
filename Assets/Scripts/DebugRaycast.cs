using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRaycast : MonoBehaviour
{
    public Vector3 direction = -Vector3.up;
    public RaycastHit hit;
    public float maxdistance = 10;
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, direction*maxdistance, Color.red);
        if(Physics.Raycast(transform.position, direction, out hit, maxdistance, layerMask))
        {
            print(hit.transform.name);
            //commands
        }
    }
}
