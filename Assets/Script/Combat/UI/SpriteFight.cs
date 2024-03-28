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

    private void OnMouseExit()
    {
        animatorOver.SetBool("over", false);
        lineRenderer.positionCount = 0;
    }

    private void OnMouseOver()
    {
        animatorOver.SetBool("over", true);
        ArrowIntention();
    }

    private void ArrowIntention()
    {
        if (character.selectedCharacters == null)
            return;

        float offSetArrow = 0.5f;
        float offSetArrow2 =0.5f;
        foreach (Character target in character.selectedCharacters)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, this.transform.position);
            Vector3 oscilation = new Vector3(Mathf.Sin(Time.time * offSetArrow) * offSetArrow2, Mathf.Sin(Time.time * offSetArrow) * offSetArrow2, -1);
            lineRenderer.SetPosition(1, PlayerCombatManager.instance.dic_CharacterSpriteFight[target].transform.position + oscilation);
        }
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
