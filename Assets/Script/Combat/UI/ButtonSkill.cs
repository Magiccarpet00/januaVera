using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class ButtonSkill : MonoBehaviour
{
    public SkillData skillData;
    public GameObject templateAttack;
    public GameObject templateParry;
    public GameObject templaceHeal;
    public MyObject myObjectParent;

    public Button button;
    public Image image;

    public void Click()
    {
        PlayerCombatManager.instance.ClickButtonSkill(skillData, myObjectParent);
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

        if (!GameManager.instance.playerCharacter.RequirementSkill(skillData))
        {
            image.color = Color.red;
            button.interactable = false;
        }
    }
}
