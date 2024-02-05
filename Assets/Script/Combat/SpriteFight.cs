using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFight : MonoBehaviour
{
    private Character character;

    //OVER
    public SpriteRenderer over;
    public Animator mainAnimator;
    public Animator animatorOver;

    private void OnMouseExit()
    {
        animatorOver.SetBool("over", false);
    }

    private void OnMouseOver()
    {
        animatorOver.SetBool("over", true);
    }

    private void OnMouseUp()
    {
        if (PlayerCombatManager.instance.inTargetMode) 
        {
            animatorOver.SetTrigger("selected");
            //CombatManager.instance.targetedCharacter.Add(character);
            GameManager.instance.playerCharacter.selectedCharacters.Add(character);
            PlayerCombatManager.instance.MinusTarget();
        }
    }

    public void AnimAtk()
    {
        mainAnimator.SetTrigger("atk");
    }

    public void ResetSelected()
    {
        animatorOver.SetTrigger("reset");
    }

    public void AnimDie()
    {
        mainAnimator.SetTrigger("die");
    }

    public void AnimDieUI()
    {
        PlayerCombatManager.instance.dic_CharacterCombatSpot[character].SetActiveSpotUI(false);
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
