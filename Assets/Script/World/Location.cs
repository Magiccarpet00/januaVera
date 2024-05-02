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

        spriteRenderer.sprite = locationData.sprite;
    }

    public List<ButtonType> GetAction()
    {
        return locationData.actionsButton;
    }
}
