using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class CombatSpot : MonoBehaviour
{
    public Character character;

    public GameObject intention;
    public TextMeshProUGUI intentionTxt;

    public GameObject lifebars;

    public GameObject prefabLifeLine;
    public List<GameObject> allLinesInSpot = new List<GameObject>();
    
    //public Slider lifeSlider;
    //public TextMeshProUGUI lifeTxt;

    public void UpdateUI()
    {
        if (character != null)
        {
            UpdateLife();
            if (character != GameManager.instance.playerCharacter)
                UpdateIntention();
        }
    }

    public void UpdateEndRoundUI()
    {
        if (character != null && character.isDead)
            character = null;

        foreach (GameObject item in allLinesInSpot)
            Destroy(item);
        allLinesInSpot.Clear();
    }

    public void UpdateIntention()
    {
        if (character.currentLoadedSkill == null || 
            character.selectedCharacters == null || 
            character.selectedCharacters.Contains(character)) 
        {
            intentionTxt.text = " ";
        }
        else
        {
            switch (character.currentLoadedSkill.skillType)
            {
                case SkillType.ATTACK:
                    SkillAttackData loadedAttackSkill = (SkillAttackData)character.currentLoadedSkill;
                    intentionTxt.text = loadedAttackSkill.damage.ToString() + " " + loadedAttackSkill.damageType.ToString();
                    break;
                case SkillType.PARRY:
                    SkillParryData loadedParrySkill = (SkillParryData)character.currentLoadedSkill;
                    intentionTxt.text = loadedParrySkill.parryType.ToString() + " " + loadedParrySkill.parryDamageType.ToString();
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


    }

    public void UpdateLife()
    {
        foreach (GameObject item in allLinesInSpot)
            Destroy(item);

        GameObject trueLifeLine = Instantiate(prefabLifeLine, transform.position, Quaternion.identity);
        trueLifeLine.transform.SetParent(lifebars.transform);
        allLinesInSpot.Add(trueLifeLine);
        LifeLine lifeLine = trueLifeLine.GetComponent<LifeLine>();
        lifeLine.lifeSlider.maxValue = character.s_VITALITY;
        lifeLine.lifeSlider.value = character.c_VITALITY;
        lifeLine.lifeTxt.text = character.c_VITALITY + "/" + character.s_VITALITY;
        lifeLine.shapeTxt.text = character.characterData.shape.ToString();
        lifeLine.lifeColor[0].color = new Color(lifeLine.trueLifeRef.r, lifeLine.trueLifeRef.g, lifeLine.trueLifeRef.b, 0.75f);
        lifeLine.lifeColor[1].color = new Color(lifeLine.trueLifeRef.r, lifeLine.trueLifeRef.g, lifeLine.trueLifeRef.b, 0.40f);

        foreach (Armor armor in character.armorsEquiped)
        {
            GameObject armorLifeLine = Instantiate(prefabLifeLine, transform.position, Quaternion.identity);
            armorLifeLine.transform.SetParent(lifebars.transform);
            allLinesInSpot.Add(armorLifeLine);
            LifeLine armorLine = armorLifeLine.GetComponent<LifeLine>();
            armorLine.lifeSlider.maxValue = armor.s_STATE;
            armorLine.lifeSlider.value = armor.c_STATE;
            armorLine.lifeTxt.text = armor.c_STATE + "/" + armor.s_STATE;
            armorLine.shapeTxt.text = armor.objectData.material.ToString();
            armorLine.lifeColor[0].color = new Color(armorLine.armorLifeRef.r, armorLine.armorLifeRef.g, armorLine.armorLifeRef.b, 0.75f);
            armorLine.lifeColor[1].color = new Color(armorLine.armorLifeRef.r, armorLine.armorLifeRef.g, armorLine.armorLifeRef.b, 0.40f);
        }
    }

    public void SetActiveSpotUI(bool b)
    {
        intentionTxt.gameObject.SetActive(b);
    }


}
