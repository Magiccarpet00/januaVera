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
    public GameObject templaceSummon;
    public MyObject myObjectParent;

    public Button button;
    public Image image;

    public void Click()
    {
        if (PlayerCombatManager.instance.inputBlock) return;
        PlayerCombatManager.instance.ClickButtonSkill(skillData, myObjectParent);
    }

    public void SetUpUI(SkillData _skillData, bool activeWeapon = false)
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
                templaceSummon.SetActive(true);
                templaceSummon.GetComponent<ButtonSkillTemplateSummon>().SetUpUI(skillData);
                break;
        }

        if (!GameManager.instance.playerCharacter.RequirementSkill(skillData))
        {
            image.color = Color.red;
            button.interactable = false;
        }

        if (!activeWeapon)
            DisableButton();

    }

    public void DisableButton()
    {
        TextMeshProUGUI[] allText = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI textMeshProUGUI in allText)
        {
            textMeshProUGUI.color = new Color(1f, 1f, 1f);
        }

        button.interactable = false;
        image.enabled = false;
    }
}
