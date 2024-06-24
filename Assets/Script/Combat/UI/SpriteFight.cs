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
    public LineRenderer lineRenderer;

    [System.Obsolete]
    private void OnMouseExit()
    {
        if (PlayerCombatManager.instance.inputBlock) return;

        animatorOver.SetBool("over", false);
        lineRenderer.enabled = false;

        if (GameManager.instance.playerCharacter.currentLoadedSkill != null &&
            PlayerCombatManager.instance.buttonSkillBuffer.skillData.skillType == SkillType.ATTACK &&
            PlayerCombatManager.instance.buttonEnd.active == false)
            PlayerCombatManager.instance.buttonSkillBuffer.templateAttack.GetComponent<ButtonSkillTemplateAttack>().ColorSwitch(false);

        
    }

    private void OnMouseOver()
    {
        if (PlayerCombatManager.instance.inputBlock) return;

        animatorOver.SetBool("over", true);
        ArrowIntention();
    }

    private void OnMouseEnter()
    {
        if (PlayerCombatManager.instance.inputBlock) return;

        if (GameManager.instance.playerCharacter.currentLoadedSkill != null && 
           PlayerCombatManager.instance.buttonSkillBuffer.skillData.skillType == SkillType.ATTACK)
            PlayerCombatManager.instance.buttonSkillBuffer.templateAttack.GetComponent<ButtonSkillTemplateAttack>().UpdateDamageInfo(character);
    }

    private void ArrowIntention()
    {
        if (character.selectedCharacters == null)
            return;

        lineRenderer.enabled = true;
        foreach (Character target in character.selectedCharacters)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, this.transform.position);
            lineRenderer.SetPosition(1, PlayerCombatManager.instance.dic_CharacterSpriteFight[target].transform.position);
        }
    }

    private void OnMouseUp()
    {
        if (PlayerCombatManager.instance.inTargetMode) 
        {
            animatorOver.SetTrigger("selected");
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
