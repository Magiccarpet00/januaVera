using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationData", menuName = "JanuaVera/Location Data")]
public class LocationData : ScriptableObject
{
    public LocationType locationType;
    public List<ButtonType> actionsButton = new List<ButtonType>();
    public Sprite sprite;

    public List<CharacterData> charactersInLocation = new List<CharacterData>();


    public List<ObjectData> objectInLocation = new List<ObjectData>();
    public List<float> rngObjet = new List<float>();
}
