using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    public LocationData locationData;

    void Start()
    {

    }

    public List<ButtonType> GetAction()
    {
        return locationData.actionsButton;
    }
}
