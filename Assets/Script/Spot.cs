using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    [SerializeField] private List<Spot> adjacentSpots = new List<Spot>();
    [SerializeField] private List<Spot> adjacentSecretSpots = new List<Spot>();
}
