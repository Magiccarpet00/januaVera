using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class ButtonSkill : MonoBehaviour
{
    [Header("Global")]
    public LocalizeStringEvent nameValue;
    public SkillData skillData;

    [Header("Attack")]
    public GameObject templateAttack;
    public TextMeshProUGUI damageValue;
    public LocalizeStringEvent damageTypeValue;
    public LocalizeStringEvent rangeTypeValue;
    public TextMeshProUGUI speedValue;
    public TextMeshProUGUI targetValue;

    public void Click()
    {
        CombatManager.instance.ClickButtonSkill(skillData);
    }

    public void SetUpUI(SkillData _skillData)
    {
        skillData = _skillData;
        nameValue.SetEntry(skillData.name);

        switch (skillData.skillType)
        {
            case SkillType.ATTACK:
                templateAttack.SetActive(true);
                damageValue.text = skillData.damage.ToString();
                damageTypeValue.SetEntry(skillData.damageType.ToString());
                rangeTypeValue.SetEntry(skillData.range.ToString());
                speedValue.text = skillData.speed.ToString();
                targetValue.text = skillData.nbTarget.ToString();
                break;

            case SkillType.PARRY:
                break;

            case SkillType.SUMMON:
                break;
        }


    }
}
