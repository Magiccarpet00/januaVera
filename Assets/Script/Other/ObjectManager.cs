using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;

    private void Awake()
    {
        instance = this;
    }

    public List<WeaponData> weaponDatas = new List<WeaponData>();
    public List<LocationData> locationDatas = new List<LocationData>();
    public List<CharacterData> characterDatas = new List<CharacterData>();


    public WeaponData FindWeaponData(string name)
    {
        WeaponData res = null;
        foreach (WeaponData d in weaponDatas)
        {
            if (d.name.Equals(name))
                res = d;
        }
        return res;
    }

    public LocationData FindLocationData(string name)
    {
        LocationData res = null;
        foreach (LocationData d in locationDatas)
        {
            if (d.name.Equals(name))
                res = d;
        }
        return res;
    }

    public CharacterData FindCharacterData(string name)
    {
        CharacterData res = null;
        foreach (CharacterData d in characterDatas)
        {
            if (d.name.Equals(name))
                res = d;
        }
        return res;
    }



}
