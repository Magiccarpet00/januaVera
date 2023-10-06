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
    private List<GameObject> charactersSprites = new List<GameObject>();

    [SerializeField] private List<GameObject> combatSpots = new List<GameObject>();
    private bool onFight;
    [SerializeField] private Vector3 posFight;
    [SerializeField] private GameObject prefabSpriteCharacter;


    //Button
    [SerializeField] private GameObject buttonAtk;

    public bool inTargetMode;
    public List<Character> targetedCharacter = new List<Character>(); // La liste actuelle des characters selectionné
    public int nbTarget; 

    public void FillSpot()
    {
        int countSpot = 0;
        foreach (Character c in characters)
        {
            combatSpots[countSpot].GetComponent<CombatSpot>().character = c;
            GameObject characterSprite = Instantiate(prefabSpriteCharacter, combatSpots[countSpot].transform.position, Quaternion.identity);
            characterSprite.GetComponent<SpriteFight>().SetCharacter(c);
            charactersSprites.Add(characterSprite);
            characterSprite.GetComponent<SpriteRenderer>().sprite = characters[countSpot].characterData.spriteFight;

            countSpot++;
        }
    }

    public void ClickButtonAtk()
    {
        TargetMode(1);
    }

    public void ClickEndButton()
    {

    }

    public void TargetMode(int _nbTarget)
    {
        nbTarget = _nbTarget;
        inTargetMode = true;
    }

    public void MinusTarget() // Decrementation du nombre de target
    {
        nbTarget--;
        if (nbTarget == 0)
            inTargetMode = false;
    }

    public void LoadCharacter(List<Character> _characters)
    {
        characters = _characters;
        FillSpot();
    }

    public void ToggleFight()
    {
        if (onFight == false) onFight = true;
        else onFight = false;
    }

    public void ClearCombatScene()
    {
        // characters.Clear();
        characters = new List<Character>();
        
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