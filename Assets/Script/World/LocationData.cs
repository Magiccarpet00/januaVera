using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationData", menuName = "JanuaVera/Location Data")]
public class LocationData : ScriptableObject
{
    public LocationType locationType;
    public List<ButtonType> actionsButton = new List<ButtonType>();
    public Sprite sprite;
}
