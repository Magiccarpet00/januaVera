using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class CombatSpot : MonoBehaviour
{
    public Character character;
    public Slider lifeSlider;
    public TextMeshProUGUI lifeTxt;
    public TextMeshProUGUI intentionTxt;

    public void UpdateUI()
    {
        if(lifeTxt.gameObject.activeSelf)
        {
            UpdateLife();
            if (character != GameManager.instance.playerCharacter)
                UpdateIntention();
        }
    }

    public void UpdateIntention()
    {
        if (character.currentLoadedSkill == null) 
            return;

        switch (character.currentLoadedSkill.skillType)
        {
            case SkillType.ATTACK:
                SkillAttackData loadedAttackSkill = (SkillAttackData)character.currentLoadedSkill;
                intentionTxt.text = loadedAttackSkill.damage.ToString() + " " + loadedAttackSkill.damageType.ToString();
                break;
            case SkillType.PARRY:
                SkillParryData loadedParrySkill = (SkillParryData)character.currentLoadedSkill;
                intentionTxt.text = loadedParrySkill.parryType.ToString() + " " + loadedParrySkill.damageType.ToString();
                break;
            case SkillType.SUMMON:
                //SkillSummonData loadedSkill = (SkillAttackData)character.currentLoadedSkill;
                //intentionTxt.text = loadedSkill.damage.ToString() + " " + loadedSkill.damageType.ToString() + "\n" + "speed :" + loadedSkill.speed.ToString();
                break;
            case SkillType.HEAL:
                SkillHealData loadedHealSkill = (SkillHealData)character.currentLoadedSkill;
                intentionTxt.text = loadedHealSkill.amount.ToString() + " " + "HEAL"; //[CODE CARNAGE]
                break;
        }
        intentionTxt.text += "\n" + "speed :" + character.currentLoadedSkill.speed.ToString();
    }

    public void UpdateLife()
    {
        lifeSlider.maxValue = character.s_VITALITY;
        lifeSlider.value = character.c_VITALITY;
        lifeTxt.text = character.c_VITALITY + "/" + character.s_VITALITY;
    }

    public void SetActiveSpotUI(bool b)
    {
        lifeSlider.gameObject.SetActive(b);
        lifeTxt.gameObject.SetActive(b);
        intentionTxt.gameObject.SetActive(b);
    }


}
