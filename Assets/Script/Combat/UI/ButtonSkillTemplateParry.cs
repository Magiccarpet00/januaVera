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
        nameValue.SetEntry(skillData.name);
        blockTypeValue.SetEntry(skillData.damageTypeParryable.ToString());
        damageValue.text = skillData.damage.ToString();
        speedValue.text = skillData.speed.ToString();
    }


}
