using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    void Awake()
    {
        instance = this;
    }

    [SerializeField] private List<Character> characters = new List<Character>();
    [SerializeField] private List<CombatSpot> combatSpots = new List<CombatSpot>();
    private bool onFight;


    [System.Obsolete]
    private void Update()
    {
        
    }


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

    public void LoadCharacter(List<Character> _characters)
    {
        characters = _characters;
    }

    public void ToggleFight()
    {
        if (onFight == false) onFight = true;
        else onFight = false;
    }

    

    


    //
    //      GET && SET
    //
    public bool GetOnFight()
    {
        return onFight;
    }
}