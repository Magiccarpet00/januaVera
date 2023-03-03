using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    [SerializeField] private List<GameObject> adjacentSpots = new List<GameObject>();
    [SerializeField] private List<GameObject> adjacentSecretSpots = new List<GameObject>();
    
    //FX
    [SerializeField] private GameObject prefabOverMouse_fx;
    private Animator AnimatorOverMouse_fx;

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
    void OnMouseEnter()
    {
        GameObject over = Instantiate(prefabOverMouse_fx, this.transform.position, Quaternion.identity);
        AnimatorOverMouse_fx = over.GetComponent<Animator>();
    }
    void OnMouseExit()
    {
        AnimatorOverMouse_fx.SetTrigger("end");
    }






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

        //TODO Les btn des enemies, pnj sur la map

        return res;
    }

}
