using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillSummonData", menuName = "JanuaVera/Skill Summon Data")]
public class SkillSummonData : SkillData
{
    public List<CharacterData> characters;
    public List<ObjectData> myObjects;
}
