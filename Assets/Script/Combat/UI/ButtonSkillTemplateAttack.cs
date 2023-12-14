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

    public LocalizeStringEvent rangeTypeValue;

    public TextMeshProUGUI speedValue;

    public TextMeshProUGUI targetValue;

    public void SetUpUI(SkillData skillData)
    {
        nameValue.SetEntry(skillData.name);
        
        damageValue.text = skillData.damage.ToString();
        damageTypeValue.SetEntry(skillData.damageType.ToString());
        rangeTypeValue.SetEntry(skillData.range.ToString());
        speedValue.text = skillData.speed.ToString();
        targetValue.text = skillData.nbTarget.ToString();
    }


}
