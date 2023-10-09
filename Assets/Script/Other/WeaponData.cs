using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "JanuaVera/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public WeaponStyle style;
    public int maxState;
    public Element material;
}
