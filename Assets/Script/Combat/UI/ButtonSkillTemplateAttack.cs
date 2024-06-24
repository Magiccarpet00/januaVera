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
    public SkillAttackData skillAttackData;

    public override void SetUpUI(SkillData skillData)
    {
        base.SetUpUI(skillData);

        skillAttackData = (SkillAttackData)skillData;
        damageValue.text = skillAttackData.damage.ToString();
        damageTypeValue.SetEntry(skillAttackData.damageType.ToString());
        speedValue.text = skillAttackData.speed.ToString();
        targetValue.text = skillAttackData.nbTarget.ToString();
    }

    public void UpdateDamageInfo(Character characterTarget)
    {
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


        if (skillAttackData.damage != amountModified) 
        {
            damageValue.text = amountModified.ToString();
            ColorSwitch(true);
        }

    }

    public void ColorSwitch(bool isChange)
    {
        if(isChange)
        {
            damageValue.color = new Color(1.0f, 0.0f, 0.0f);
            damageValue.fontSize = 15.0f;
            damageValue.fontStyle = FontStyles.Bold;
        }
        else
        {
            damageValue.color = new Color(0.20f, 0.20f, 0.20f);
            damageValue.fontSize = 10.0f;
            damageValue.fontStyle = FontStyles.Normal;
            damageValue.text = skillAttackData.damage.ToString();
        }
    }


}
