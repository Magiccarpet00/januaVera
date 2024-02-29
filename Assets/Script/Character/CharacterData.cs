using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "JanuaVera/Character Data")]
public class CharacterData : ScriptableObject
{
    public Race race;
    public Element shape;
    public Divinity typeDivinity; //uniquement si le character est divin

    public int init_VITALITY, init_ENDURANCE, init_STRENGHT, init_DEXTERITY, init_FAITH;

    public Sprite spriteMap;
    public Sprite spriteFight;
}
