using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFight : MonoBehaviour
{
    private Character character;

    void OnMouseEnter()
    {
        
    }

    private void OnMouseUp()
    {
        Debug.Log("OnMouseUp");
        if (CombatManager.instance.inTargetMode) 
        {
            Debug.Log("TargetMod");
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
