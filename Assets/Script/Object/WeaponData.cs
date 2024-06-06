using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "JanuaVera/Weapon Data")]
public class WeaponData : ObjectData
{
    //Le nom du scriptable object doit etre en majuscule
    public WeaponStyle style;
    public List<SkillData> skills = new List<SkillData>();
    public int nbHand;
}
