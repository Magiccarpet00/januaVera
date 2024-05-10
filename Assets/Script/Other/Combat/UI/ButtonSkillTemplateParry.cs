using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class ButtonSkillTemplateParry : ButtonSkillTemplate
{
    public TextMeshProUGUI blockTypeValue;
    public TextMeshProUGUI damageValue;
    public override void SetUpUI(SkillData skillData)
    {
        base.SetUpUI(skillData);

        SkillParryData skillParryData = (SkillParryData)skillData;

        blockTypeValue.text = "";
        foreach (DamageType item in skillParryData.parryDamageType)
            blockTypeValue.text += item.ToString() + "\n";

        damageValue.text = skillParryData.damage.ToString();
    }
}
