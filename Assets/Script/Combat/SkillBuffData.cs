using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillBuffData", menuName = "JanuaVera/Skill Buff Data")]
public class SkillBuffData : SkillData
{
    

    public int nbRound; //Round -> Fight
    public int nbTurn;  //Turn  -> Map

    public int mod_VITALITY, mod_ENDURANCE, mod_STRENGHT, mod_DEXTERITY, mod_FAITH,
               mod_SPEED, mod_DAMAGE;


}
