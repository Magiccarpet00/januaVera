using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    [SerializeField] private List<GameObject> adjacentSpots = new List<GameObject>();
    [SerializeField] private List<GameObject> adjacentSecretSpots = new List<GameObject>();


    public void AddAdjacentSpots(GameObject newSpot, bool secretSpot)
    {
        if (secretSpot)
            adjacentSecretSpots.Add(newSpot);
        else
            adjacentSpots.Add(newSpot);
    }

    public List<GameObject> GetAdjacentSpots()
    {
        return adjacentSpots;
    }

    void OnMouseEnter()
    {
        //TODO FX
        Debug.Log("Mouse is over GameObject.");
    }
}
