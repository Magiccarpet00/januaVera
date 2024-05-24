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

}
