using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class ButtonSkillTemplateHeal : MonoBehaviour
{
    public LocalizeStringEvent nameValue;
    public TextMeshProUGUI damageValue;
    public TextMeshProUGUI speedValue;
    public TextMeshProUGUI targetValue;

    public void SetUpUI(SkillData skillData)
    {
        SkillHealData skillhealData = (SkillHealData)skillData;

        nameValue.SetEntry(skillhealData.name);

        damageValue.text = skillhealData.amount.ToString();
        speedValue.text = skillhealData.speed.ToString();
        targetValue.text = skillhealData.nbTarget.ToString();
    }


}
