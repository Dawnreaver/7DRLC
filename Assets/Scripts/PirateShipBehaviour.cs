using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateShipBehaviour : MonoBehaviour
{
    public GameObject preyObject;
    public Vector2 tileOrigin = new Vector2();
    public bool sightedPrey = false;
    private float m_distanceToPrey;
    private int m_ketchUpEstimate = 0;
    public int followThreshold = 15; // number of fields the pirate will follow beforr abandoning the chase

}
