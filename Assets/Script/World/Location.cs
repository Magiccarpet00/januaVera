using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    public LocationData locationData;
    public SpriteRenderer spriteRenderer;


    public void SetUpLocation(GameObject spot)
    {
        foreach (CharacterData characterData in locationData.charactersInLocation)
            GameManager.instance.CreateCharacter(characterData, spot);


        for (int i = 0; i < locationData.objectInLocation.Count; i++)
        {
            float rng = Random.Range(0f, 1f);
            if(rng <= locationData.rngObjet[i])
                spot.GetComponent<Spot>().AddObject(GameManager.instance.CreateObject(locationData.objectInLocation[i]));
        }

        spriteRenderer.sprite = locationData.sprite;
    }

    public List<ButtonType> GetAction()
    {
        return locationData.actionsButton;
    }
}
