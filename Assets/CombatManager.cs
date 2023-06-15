using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    void Awake()
    {
        instance = this;
    }

    [SerializeField] private List<Character> characters = new List<Character>();
    [SerializeField] private List<CombatSpot> combatSpots = new List<CombatSpot>();

    public void FillSpot()
    {
        int countSpot = 0;
        foreach (Character c in characters)
        {   
            for (int i = 0; i < 3; i++)
            {
                combatSpots[countSpot].characters.Add(c);
            }
        }
    }




}
