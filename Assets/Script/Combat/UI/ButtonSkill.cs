using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class ButtonSkill : MonoBehaviour
{
    public SkillData skillData;
    public GameObject templateAttack;
    public GameObject templateParry;
    public GameObject templaceHeal;

    public void Click()
    {
        PlayerCombatManager.instance.ClickButtonSkill(skillData);
    }

    public void SetUpUI(SkillData _skillData)
    {
        skillData = _skillData;

        switch (skillData.skillType)
        {
            case SkillType.ATTACK:
                templateAttack.SetActive(true);
                templateAttack.GetComponent<ButtonSkillTemplateAttack>().SetUpUI(skillData);
                break;

            case SkillType.PARRY:
                templateParry.SetActive(true);
                templateParry.GetComponent<ButtonSkillTemplateParry>().SetUpUI(skillData);
                break;

            case SkillType.HEAL:
                templaceHeal.SetActive(true);
                templaceHeal.GetComponent<ButtonSkillTemplateHeal>().SetUpUI(skillData);
                break;

            case SkillType.SUMMON:
                break;
        }


    }
}
