using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "JanuaVera/Skill Data")]
public class SkillData : ScriptableObject
{
    public SkillType skillType; //[CODE DESEPOIRE] Je n'arrive pas a faire une systeme agreable sans enlever ce truc
    public Element element;
    public int speed;
    public int nbTarget;
    public int req_VITALITY, req_ENDURANCE, req_STRENGHT, req_DEXTERITY, req_FAITH;
}
