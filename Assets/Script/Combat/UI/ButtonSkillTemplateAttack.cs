using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class ButtonSkillTemplateAttack : MonoBehaviour
{
    public LocalizeStringEvent nameValue;

    public TextMeshProUGUI damageValue;
    public LocalizeStringEvent damageTypeValue;

    public TextMeshProUGUI speedValue;

    public TextMeshProUGUI targetValue;

    public TextMeshProUGUI requirementValue;

    public void SetUpUI(SkillData skillData)
    {
        SkillAttackData skillAttackData = (SkillAttackData)skillData;
        
        nameValue.SetEntry(skillAttackData.name);
        damageValue.text = skillAttackData.damage.ToString();
        damageTypeValue.SetEntry(skillAttackData.damageType.ToString());
        speedValue.text = skillAttackData.speed.ToString();
        targetValue.text = skillAttackData.nbTarget.ToString();

        //TMP
        string reqValue = "";
        if(skillAttackData.req_VITALITY != 0)
           reqValue += " VIT >= " + skillAttackData.req_VITALITY + " ";
        if(skillAttackData.req_ENDURANCE != 0)
           reqValue += " END >= " + skillAttackData.req_ENDURANCE + " ";
        if(skillAttackData.req_FAITH != 0)
           reqValue += " FAI >= " + skillAttackData.req_FAITH + " ";
        if(skillAttackData.req_STRENGHT != 0)
           reqValue += " STR >= " + skillAttackData.req_STRENGHT + " ";
        if(skillAttackData.req_DEXTERITY != 0)
           reqValue += " DEX >= " + skillAttackData.req_DEXTERITY + " ";
        requirementValue.text = reqValue;
    }


}
