using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class ButtonSkillTemplateHeal : ButtonSkillTemplate
{
    public TextMeshProUGUI damageValue;
    public TextMeshProUGUI targetValue;

    public override void SetUpUI(SkillData skillData)
    {
        base.SetUpUI(skillData);

        SkillHealData skillhealData = (SkillHealData)skillData;
        damageValue.text = skillhealData.amount.ToString();
        targetValue.text = skillhealData.nbTarget.ToString();
    }

}
