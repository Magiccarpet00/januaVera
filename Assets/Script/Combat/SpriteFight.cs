using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFight : MonoBehaviour
{
    private Character character;
    public bool selected;

    //OVER
    public SpriteRenderer over;
    public Animator animatorOver;

    private void OnMouseExit()
    {
        if(!selected)
            over.gameObject.SetActive(false);
    }

    private void OnMouseOver()
    {
        over.gameObject.SetActive(true);
    }

    private void OnMouseUp()
    {
        if (CombatManager.instance.inTargetMode) 
        {
            selected = true;
            animatorOver.SetTrigger("selected");
            CombatManager.instance.targetedCharacter.Add(character);
            CombatManager.instance.MinusTarget();
        }
    }

    public void ResetSelected()
    {
        animatorOver.SetTrigger("refresh");
        //over.gameObject.SetActive(false);
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
