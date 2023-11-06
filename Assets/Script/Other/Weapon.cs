using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public WeaponData weaponData;
    public int currentState;

    public Weapon(WeaponData wd)
    {
        weaponData = wd;
        currentState = weaponData.maxState;
    }

}
