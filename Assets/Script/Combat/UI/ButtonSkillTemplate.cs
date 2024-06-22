using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class ButtonSkillTemplate : MonoBehaviour
{
    public LocalizeStringEvent nameValue;
    public TextMeshProUGUI speedValue;
    public TextMeshProUGUI requirementValue;
    public SkillData skillData;

    public virtual void SetUpUI(SkillData skillData)
    {
        this.skillData = skillData;
        nameValue.SetEntry(skillData.name);
        speedValue.text = skillData.speed.ToString();
        //TMP
        string reqValue = "";
        if (skillData.req_VITALITY != 0)
            reqValue += " -" + skillData.req_VITALITY + " VIT\n";
        if (skillData.req_ENDURANCE != 0)
            reqValue += " -" + skillData.req_ENDURANCE + " END\n";
        if (skillData.req_FAITH != 0)
            reqValue += "req FAI > " + skillData.req_FAITH + "\n";
        if (skillData.req_STRENGHT != 0)
            reqValue += "req STR > " + skillData.req_STRENGHT + "\n";
        if (skillData.req_DEXTERITY != 0)
            reqValue += "req DEX > " + skillData.req_DEXTERITY + "\n";
        requirementValue.text = reqValue;

        if (GameManager.instance.playerCharacter.RequirementSkill(skillData))
            requirementValue.color = new Color(0.0f, 1.0f, 0.0f);
    }
}
