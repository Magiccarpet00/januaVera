using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spot : MonoBehaviour
{
    [SerializeField] private List<GameObject> adjacentSpots = new List<GameObject>();
    [SerializeField] private List<GameObject> adjacentSecretSpots = new List<GameObject>();

    [SerializeField] public Tile parrentTile;

    [SerializeField] public List<Character> charactersOnSpot = new List<Character>();

    [SerializeField] public List<MyObject> objectsOnSpot = new List<MyObject>();
    [SerializeField] public Stack<GameObject> objectsOnSpotUI = new Stack<GameObject>();

    //FX
    [SerializeField] private GameObject prefabOverMouse_fx;
    private Animator AnimatorOverMouse_fx;


    private void Update()
    {
        UpdatePosCharacter();
    }

    public void UpdatePosCharacter()
    {
        int count = 0;
        int total = GetAllCharactersAliveOnMapInSpot().Count;
        System.Numerics.Complex i = System.Numerics.Complex.ImaginaryOne;

        foreach (Character characterAlive in GetAllCharactersAliveOnMapInSpot())
        {
            float timeRotate = (Time.time * total) * 0.03f;
            characterAlive.offSetOnSpot.x = (float)System.Numerics.Complex.Exp((2 * Mathf.PI * (count + timeRotate) * i) / total).Real;
            characterAlive.offSetOnSpot.y = (float)System.Numerics.Complex.Exp((2 * Mathf.PI * (count + timeRotate) * i) / total).Imaginary;
            count++;
        }
    }

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





    //
    //      FX
    //
    
    //[CODE PLACARD] Je ne suis pas sur de cette fonctionaliter.

    //void OnMouseEnter()
    //{
    //    GameObject over = Instantiate(prefabOverMouse_fx, this.transform.position, Quaternion.identity);
    //    AnimatorOverMouse_fx = over.GetComponent<Animator>();
    //    GameManager.instance.CreateInfoGridLayoutGroupe(this.transform.position, charactersOnSpot);
    //}
    //void OnMouseExit()
    //{
    //    AnimatorOverMouse_fx.SetTrigger("end");
    //    GameManager.instance.DestroyInfoGridLayoutGroupe();
    //}

    private void OnMouseUpAsButton()
    {
        if(GameManager.instance.inputBlock == false)
        {
            GameManager.instance.playerCharacter.CommandMove(this.gameObject);
            GameManager.instance._ExecuteActionQueue();
        }
    }


    //
    //      RELATION
    //



    //
    //      GET & SET
    //
    public LocationType GetLocationInSpot()
    {
        LocationType res = LocationType.EMPTY;

        if (transform.childCount == 1)
        {
            Location selfLocation = GetComponentInChildren<Location>();
            res = selfLocation.locationData.locationType;
        }

        return res;
    }

    public List<ButtonType> GetActionInSpot() 
    {
        List<ButtonType> res = new List<ButtonType>();

        res.AddRange(GameManager.instance.GetStandarActionButton()); // Les btn de base 

        if (GetLocationInSpot() != LocationType.EMPTY)               // Les btn de la location
        {
            Location selfLocation = GetComponentInChildren<Location>();
            res.AddRange(selfLocation.GetAction());
        }

        foreach (Character character in GetAllCharactersInSpot())
        {
            if (character.characterData.isMerchant && !character.isDead && !res.Contains(ButtonType.TRADE))
                res.Add(ButtonType.TRADE);

            if (character.characterData.workCost != 0 && !character.isDead && !res.Contains(ButtonType.HIRE))
                res.Add(ButtonType.HIRE);
        }

        if (objectsOnSpot.Count != 0 && !res.Contains(ButtonType.SEARCH))
            res.Add(ButtonType.SEARCH);

        

        //TODO Les btn des enemies, pnj sur la map

        return res;
    }

    public List<Character> GetAllCharactersInSpot()
    {
        return charactersOnSpot;
    }

    public List<Character> GetAllCharactersAliveInSpot()
    {
        List<Character> charactersInSpot = GetAllCharactersInSpot();
        List<Character> charactersAlive = new List<Character>();
        foreach (Character character in charactersInSpot)
            if (!character.isDead)
                charactersAlive.Add(character);
        return charactersAlive;
    }

    public List<Character> GetAllCharactersAliveOnMapInSpot()
    {
        List<Character> charactersInSpot = GetAllCharactersInSpot();
        List<Character> charactersAlive = new List<Character>();
        foreach (Character character in charactersInSpot)
            if (!character.isDead && !character.characterData.inLocation)
                charactersAlive.Add(character);
        return charactersAlive;
    }

    public List<Character> GetAllCharactersOnMapInSpot()
    {
        List<Character> charactersInSpot = GetAllCharactersInSpot();
        List<Character> charactersAlive = new List<Character>();
        foreach (Character character in charactersInSpot)
            if (!character.characterData.inLocation)
                charactersAlive.Add(character);
        return charactersAlive;
    }

    public void AddCharacterInSpot(Character c)
    {
        charactersOnSpot.Add(c);
    }

    public void RemoveCharacterInSpot(Character c)
    {
        charactersOnSpot.Remove(c);
    }

    public void AddObject(MyObject obj)
    {
        objectsOnSpot.Add(obj);
        float xOff = 0.4f;
        Vector3 offSet = new Vector3(objectsOnSpot.Count*xOff - xOff, 0f, 0f);
        GameObject objectUI = Instantiate(GameManager.instance.prefabObjectOnMap, transform.position + offSet, Quaternion.identity);
        objectsOnSpotUI.Push(objectUI);
    }

    public MyObject TakeObject()
    {
        if (objectsOnSpot.Count == 0)
            return null;
        MyObject obj;
        obj = objectsOnSpot[objectsOnSpot.Count-1];
        objectsOnSpot.RemoveAt(objectsOnSpot.Count-1);
        GameObject objectUI = objectsOnSpotUI.Pop();
        Destroy(objectUI);
        return obj;
    }

}
