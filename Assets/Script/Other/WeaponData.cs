using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "JanuaVera/Weapon Data")]
public class WeaponData : ScriptableObject
{
    //Le nom du scriptable object doit etre en majuscule
    public WeaponStyle style;
    public int maxState;
    public Element material;
    public List<SkillData> skills = new List<SkillData>();
}
