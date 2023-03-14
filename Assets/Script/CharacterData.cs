using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "JanuaVera/Character Data")]
public class CharacterData : ScriptableObject
{
    public Element shape;
    public Divinity typeDivinity; //uniquement si le character est divin
    public int shape_value;

    

    public int stamina;
    public int nbWound;

    public int dodgeTime;
    

}
