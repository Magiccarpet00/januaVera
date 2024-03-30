using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class ButtonSkillTemplateParry : MonoBehaviour
{
    public LocalizeStringEvent nameValue;
    public LocalizeStringEvent blockTypeValue;
    public TextMeshProUGUI damageValue;
    public TextMeshProUGUI speedValue;
    public void SetUpUI(SkillData skillData)
    {
        SkillParryData skillParryData = (SkillParryData)skillData;

        nameValue.SetEntry(skillParryData.name);
        blockTypeValue.SetEntry(skillParryData.damageType.ToString());
        damageValue.text = skillParryData.damage.ToString();
        speedValue.text = skillParryData.speed.ToString();
    }
}
