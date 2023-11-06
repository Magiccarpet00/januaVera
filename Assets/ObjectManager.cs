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


    public WeaponData FindWeaponData(string name)
    {
        WeaponData res = null;

        foreach (WeaponData weaponData in weaponDatas)
        {
            if (weaponData.name.Equals(name))
                res = weaponData;
        }

        if (res == null)
            Debug.LogError("ERROR: wrong name in FindWeaponData");

        return res;
    }

}
