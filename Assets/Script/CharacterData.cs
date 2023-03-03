using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "JanuaVera/Character Data")]
public class CharacterData : ScriptableObject
{
    public Element shape;
    public int shape_value;

    public int stamina;
    public int nbWound;

    public int dodgeSpeed;

}
