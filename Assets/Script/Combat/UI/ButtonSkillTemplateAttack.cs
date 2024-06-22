using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class ButtonSkillTemplateAttack : ButtonSkillTemplate
{
    public TextMeshProUGUI damageValue;
    public LocalizeStringEvent damageTypeValue;
    public TextMeshProUGUI targetValue;

    public override void SetUpUI(SkillData skillData)
    {
        base.SetUpUI(skillData);

        SkillAttackData skillAttackData = (SkillAttackData)skillData;
        damageValue.text = skillAttackData.damage.ToString();
        damageTypeValue.SetEntry(skillAttackData.damageType.ToString());
        speedValue.text = skillAttackData.speed.ToString();
        targetValue.text = skillAttackData.nbTarget.ToString();
    }

    public void UpdateDamageInfo(Character characterTarget)
    {
        SkillAttackData skillAttackData = (SkillAttackData)skillData;
        int amountModified;

        if (characterTarget.armorsEquiped.Count != 0)
        {
            if (skillAttackData.damageType == DamageType.ELEM)
                amountModified = (int)(GameManager.instance.typeChart[(skillAttackData.element.ToString(), characterTarget.armorsEquiped[characterTarget.armorsEquiped.Count - 1].objectData.material)] * skillAttackData.damage);
            else
                amountModified = (int)(GameManager.instance.typeChart[(skillAttackData.damageType.ToString(), characterTarget.armorsEquiped[characterTarget.armorsEquiped.Count - 1].objectData.material)] * skillAttackData.damage);
        }
        else
        {
            if (skillAttackData.damageType == DamageType.ELEM)
                amountModified = (int)(GameManager.instance.typeChart[(skillAttackData.element.ToString(), characterTarget.characterData.shape)] * skillAttackData.damage);
            else
                amountModified = (int)(GameManager.instance.typeChart[(skillAttackData.damageType.ToString(), characterTarget.characterData.shape)] * skillAttackData.damage);
        }

        damageValue.text = amountModified.ToString();

        if (skillAttackData.damage != amountModified) 
        {
            damageValue.color = new Color(1.0f, 0.0f, 0.0f);
        }
        else
        {
            damageValue.color = new Color(0.0f, 0.0f, 0.0f);
        }

        //TODO continer la previsualisation des degats 


    }

}
