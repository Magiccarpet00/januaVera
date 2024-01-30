using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "JanuaVera/Character Data")]
public class CharacterData : ScriptableObject
{
    public Race race;
    public Element shape;
    public Divinity typeDivinity; //uniquement si le character est divin

    public int maxLife;
    public int stamina;
        
    public Sprite spriteMap;
    public Sprite spriteFight;
}
