using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillParryData", menuName = "JanuaVera/Skill Parry Data")]
public class SkillParryData : SkillData
{
    public ParryType parryType;

    public int damage;
    public DamageType damageType;

    public List<DamageType> parryDamageType = new List<DamageType>();
    public int nbGarde;
}
