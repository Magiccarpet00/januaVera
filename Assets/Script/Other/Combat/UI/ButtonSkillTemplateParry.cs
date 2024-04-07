using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class ButtonSkillTemplateParry : ButtonSkillTemplate
{
    public LocalizeStringEvent blockTypeValue;
    public TextMeshProUGUI damageValue;
    public override void SetUpUI(SkillData skillData)
    {
        base.SetUpUI(skillData);

        SkillParryData skillParryData = (SkillParryData)skillData;
        blockTypeValue.SetEntry(skillParryData.damageType.ToString());
        damageValue.text = skillParryData.damage.ToString();
    }
}
