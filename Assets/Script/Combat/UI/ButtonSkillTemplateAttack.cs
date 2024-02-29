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

    public TextMeshProUGUI speedValue;

    public TextMeshProUGUI targetValue;

    public void SetUpUI(SkillData skillData)
    {
        SkillAttackData skillAttackData = (SkillAttackData)skillData;

        nameValue.SetEntry(skillAttackData.name);

        damageValue.text = skillAttackData.damage.ToString();
        damageTypeValue.SetEntry(skillAttackData.damageType.ToString());
        speedValue.text = skillAttackData.speed.ToString();
        targetValue.text = skillAttackData.nbTarget.ToString();
    }


}
