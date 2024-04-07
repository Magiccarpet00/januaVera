using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    public LocationData locationData;
    public SpriteRenderer spriteRenderer;


    public void SetUpLocation(GameObject spot)
    {
        if (locationData.charactersInLocation.Count != 0) //[CODE TMP] Ne gere pas plusieurs characters
            GameManager.instance.CreateCharacter(locationData.charactersInLocation[0], spot);

        spriteRenderer.sprite = locationData.sprite;
    }

    public List<ButtonType> GetAction()
    {
        return locationData.actionsButton;
    }
}
