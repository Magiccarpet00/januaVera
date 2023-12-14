using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "JanuaVera/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("Global")]
    public SkillType skillType;
    public Element element;
    public Range range;
    public int speed;
    public int nbTarget;

    [Header("Attack")]
    public int damage;
    public DamageType damageType;

    [Header("Parry")]
    public ParryType parryType;
    public DamageType damageTypeParryable;
    public int nbGarde; 

}
