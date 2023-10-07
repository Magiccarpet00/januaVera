using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFight : MonoBehaviour
{
    private Character character;

    private void OnMouseUp()
    {
        if (CombatManager.instance.inTargetMode) 
        {
            CombatManager.instance.targetedCharacter.Add(character);
            CombatManager.instance.MinusTarget();
        }
    }

    public void SetCharacter(Character c)
    {
        character = c;
    }

    public Character GetCharacter()
    {
        return character;
    }
}
