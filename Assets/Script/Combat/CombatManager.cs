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
    [SerializeField] private Vector3 posFight;



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

    public void ClearCombatScene()
    {
        // characters.Clear();
        // [BUG RESOLUE]
        // Supprimer la list de character dans CombatManager supprime la list des characters dans spot
        // du coup je fait pas de clear mais je comprend pas pourquoi
    }






    //
    //      GET && SET
    //
    public bool GetOnFight()
    {
        return onFight;
    }

    public Vector3 GetPosFight()
    {
        return posFight;
    }
}